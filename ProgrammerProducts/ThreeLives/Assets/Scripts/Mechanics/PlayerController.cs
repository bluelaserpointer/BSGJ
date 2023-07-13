using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using System.Linq;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public static PlayerController Instance { get; private set; }

        //Inspector
        [Header("SE")]
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public AudioClip ouchSeaAudio;
        [SerializeField]
        AudioClip _plantAudio;

        [Header("Ability")]
        [SerializeField]
        PlantedVine _vineSeedPrefab;
        [SerializeField]
        FrozenFlower _frozenFlowerPrefab;
        [SerializeField]
        PlantSign _plantSignPrefab;
        [SerializeField]
        PlantRetriever _plantRetrieverPrefab;
        public List<int> avaliableItemsIndex = new List<int>(); 
        [Header("Mobility")]
        public bool controlEnabled = true;
        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Max vertical speed of climbing down.
        /// </summary>
        [SerializeField]
        float _maxClimbDownSpeed = 5;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;

        //Hide from inspector
        public Collider2D Collider2d { get; private set; }
        public Collider2D GroundCollider { get; private set; }
        public Bounds Bounds => Collider2d.bounds;
        public Health Health { get; private set; }
        public AudioSource AudioSource { get; private set; }
        public Dictionary<Collider2D, Collider2D> flowerCopyRecord = new Dictionary<Collider2D, Collider2D>();

        public int SelectedItemIndex
        {
            get => _selectedItemIndex;
            set
            {
                if (value != -1 && !avaliableItemsIndex.Contains(value))
                    return;
                _selectedItemIndex = value;
                WorldManager.Instance.GameUI.ItemBar.OnSelectedItem(value);
            }
        }
        int _selectedItemIndex;

        private bool _stopJump;
        public bool _jump;
        bool _down;
        Vector2 _move;
        Vector2 slipoffNormal;
        SpriteRenderer _spriteRenderer;
        internal Animator _animator;
        PlatformerModel Model => Simulation.GetModel<PlatformerModel>();

        [HideInInspector]
        public Interactable avaliableInteractable;

        [HideInInspector]
        public int onLadderCount;
        public bool OnLadder => onLadderCount > 0;
        List<Collider2D> _penetratingColliders = new List<Collider2D>();

        Dictionary<int, PlantSign> _idAndPlantedObjectSign = new Dictionary<int, PlantSign>();
        Dictionary<int, bool> _idAndIsRetrieving = new Dictionary<int, bool>();

        void Awake()
        {
            Instance = this;
            Health = GetComponent<Health>();
            AudioSource = GetComponent<AudioSource>();
            Collider2d = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _idAndIsRetrieving[0] = false;
            _idAndIsRetrieving[1] = false;
        }
        protected override void Start()
        {
            base.Start();
            slipoffNormal = Vector2.right;
            if(avaliableItemsIndex.Count > 0)
            {
                SelectedItemIndex = avaliableItemsIndex[0];
            }
            else
            {
                SelectedItemIndex = -1;
            }
        }

        protected override void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                avaliableInteractable?.OnInteract.Invoke();
            }
            if (Input.GetKey(KeyCode.E))
            {
                avaliableInteractable?.OnInteractStay.Invoke();
                if(avaliableInteractable)
                {
                    controlEnabled = false;                    
                }
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                if(avaliableInteractable)
                {
                    controlEnabled = true;                    
                }
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                SelectedItemIndex = 0;
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                SelectedItemIndex = 1;
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                SelectedItemIndex = 2;
            }
            else if (Input.GetKey(KeyCode.Tab))
            {
                int index = avaliableItemsIndex.IndexOf(SelectedItemIndex);
                if (index != -1)
                {
                    SelectedItemIndex = avaliableItemsIndex[(index + 1) % avaliableItemsIndex.Count];
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_idAndPlantedObjectSign.TryGetValue(SelectedItemIndex, out PlantSign plantSign))
                {
                    //retrieve old plant before planting new one
                    _idAndIsRetrieving[SelectedItemIndex] = true;
                    PlantRetriever retriever = Instantiate(_plantRetrieverPrefab);
                    retriever.transform.position = plantSign.transform.position;
                    retriever.Init(SelectedItemIndex);
                    if (plantSign.Plant != null)
                    {
                        plantSign.Plant.Destroy();
                    }
                    Destroy(plantSign.gameObject);
                    _idAndPlantedObjectSign.Remove(SelectedItemIndex);
                }
                else if (GroundCollider == null)
                {
                    //cannnot plant in mid-air;
                }
                else if (_idAndIsRetrieving[SelectedItemIndex])
                {
                    //cannnot plant until the seed is retreiving
                }
                else
                {
                    //plant new one
                    Transform seedTarget;
                    if(flowerCopyRecord != null && flowerCopyRecord.TryGetValue(GroundCollider, out Collider2D copyCollider) && copyCollider != null)
                    {
                        seedTarget = copyCollider.transform;
                    }
                    else
                    {
                        TransformOnTimeShift parentTransformNode = GroundCollider.GetComponentInParent<TransformOnTimeShift>();
                        if (parentTransformNode != null && parentTransformNode.HoldPlantedObjects)
                        {
                            seedTarget = parentTransformNode.transform;
                        }
                        else
                        {
                            seedTarget = GroundCollider.transform;
                        }
                    }
                    PlantableObject plant = null;
                    switch (SelectedItemIndex)
                    {
                        case 0:
                            plant = Instantiate(_vineSeedPrefab, seedTarget);
                            break;
                        case 1:
                            plant = Instantiate(_frozenFlowerPrefab, seedTarget);
                            break;
                        default:
                            print("<!> illegal plant index");
                            break;
                    }
                    plant.transform.position = transform.position;
                    plant.transform.localScale = new Vector3(1 / plant.transform.parent.lossyScale.x, 1 / plant.transform.parent.lossyScale.y, 1 / plant.transform.parent.lossyScale.z);
                    WorldManager.Instance.PlayOneShotSound(_plantAudio);
                    PlantSign sign = Instantiate(_plantSignPrefab);
                    sign.SetPlant(plant);
                    sign.transform.position = plant.transform.position;
                    _idAndPlantedObjectSign.Add(SelectedItemIndex, sign);
                }
            }
            if (controlEnabled)
            {
                _move.x = Input.GetAxis("Horizontal");
                _down = Input.GetAxis("Vertical") < 0;
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    _stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                _move.x = 0;
            }
            //stop ignoring not overlapped penetrating colliders
            foreach (Collider2D penetratingCollider in _penetratingColliders.ToList())
            {
                if(penetratingCollider == null || !Collider2d.Distance(penetratingCollider).isOverlapped)
                {
                    //print("penetration exit:" + penetratingCollider.gameObject.transform.parent.name);
                    _penetratingColliders.Remove(penetratingCollider);
                }
            }
            UpdateJumpState();
            base.Update();
        }
        protected override void FixedUpdate()
        {
            if (velocity.y < 0 && OnLadder && !_down) //stop falling on ladder
            {
                IsGrounded = true;
                GroundCollider = null;
                groundNormal = Vector2.up;
                slipoffNormal = Vector2.right;
            }
            else
            {
                IsGrounded = false;
                GroundCollider = null;
                //if already falling, fall faster than the jump speed, otherwise use normal gravity.
                if (velocity.y < 0)
                {
                    var fallAlongWall = new Vector2(slipoffNormal.y, -slipoffNormal.x) * (slipoffNormal.x > 0 ? -1 : 1);
                    Vector2 oldVelocity = velocity;
                    velocity += fallAlongWall * gravityModifier * Physics2D.gravity.y * Time.deltaTime;
                    //print("velocity " + oldVelocity.x + ", " + oldVelocity.y + " -> " + velocity.x + ", " + velocity.y);
                    //print(fallAlongWall + " -> " + (fallAlongWall * gravityModifier * Physics2D.gravity.y));
                    if (OnLadder && _down && velocity.y < -_maxClimbDownSpeed)
                    {
                        velocity = new Vector2(velocity.x, -_maxClimbDownSpeed);
                    }
                }
                else
                    velocity += Physics2D.gravity * Time.deltaTime;
            }

            bool slipoff = false;
            if (slipoffNormal == Vector2.right || slipoffNormal.x * targetVelocity.x > 0)
                velocity = new Vector2(targetVelocity.x, velocity.y);
            else
                slipoff = true;

            var deltaPosition = velocity * Time.deltaTime;

            //print("deltaPosition " + deltaPosition.x + ", " + deltaPosition.y + " slipoff " + slipoff);

            slipoffNormal = Vector2.right;

            Vector2 xmove;

            if(slipoff)
            {
                xmove = Vector2.right * deltaPosition.x;
            }
            else
            {
                var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
                xmove = moveAlongGround * deltaPosition.x;
            }

            PerformMovement(xmove, false);

            Vector2 ymove = Vector2.up * deltaPosition.y;
            PerformMovement(ymove, true);
            //print("xmove " + xmove.x + ", " + xmove.y + ", ymove " + ymove.x + ", " + ymove.y);

            if (!IsGrounded)
                groundNormal = Vector2.up;
        }
        public void RemoveAllPlantedObjects()
        {
            foreach(var idAndPlant in _idAndPlantedObjectSign)
            {
                if(idAndPlant.Value != null)
                    Destroy(idAndPlant.Value.gameObject);
            }
            _idAndPlantedObjectSign.Clear();
        }
        public void OnRetrievedPlant(int plantId)
        {
            _idAndIsRetrieving[plantId] = false;
        }
        protected override void PerformMovement(Vector2 move, bool yMovement)
        {
            var distance = move.magnitude;

            if (distance > minMoveDistance)
            {
                //check if we hit anything in current direction of travel
                var hitCount = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
                for (int i = 0; i < hitCount; ++i)
                {
                    RaycastHit2D hitInfo = hitBuffer[i];
                    if (hitInfo.collider.isTrigger)
                        continue;
                    if (_penetratingColliders.Contains(hitInfo.collider))
                        continue;
                    if (hitInfo.collider.GetType() == typeof(EdgeCollider2D) || (hitInfo.collider.GetComponent<Wall>()?.penetratable ?? false))
                    {   
                        if (
                            move.y > 0 //penetrate edge collider ceilings while jumping
                            || OnLadder && _down //penetrate edge coolider grounds while climb down ladders
                            )
                        {
                            //remember those colliders should be ignored while overlapping
                            //print("penetraing:" + hitInfo.collider.gameObject.transform.parent.name);
                            _penetratingColliders.Add(hitInfo.collider);
                            continue;
                        }
                    }
                    var currentNormal = hitInfo.normal;

                    //is this surface flat enough to land on?
                    if (currentNormal.y > minGroundNormalY)
                    {
                        //print(currentNormal.y + " bigger " + minGroundNormalY);
                        IsGrounded = true;
                        GroundCollider = hitInfo.collider;
                        // edit: disabled yMovement check from original
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                    else if (!IsGrounded && currentNormal.y > slipoffNormal.y)
                    {
                        slipoffNormal = currentNormal;
                    }
                    if (IsGrounded)
                    {
                        //how much of our velocity aligns with surface normal?
                        var projection = Vector2.Dot(velocity, currentNormal);
                        //slower velocity if moving against the normal (up a hill).
                        if (projection < 0)
                        {
                            velocity -= projection * currentNormal;
                        }
                    }
                    else
                    {
                        //We are airborne, but hit something, so cancel vertical up and horizontal velocity.
                        velocity = new Vector2(velocity.x, Mathf.Min(velocity.y, 0));
                    }
                    //remove shellDistance from actual move distance.
                    var modifiedDistance = hitInfo.distance - shellRadius;
                    if (modifiedDistance <= 0)
                    {
                        distance = move.magnitude;
                        move = currentNormal;
                        break;
                    }
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            Vector2 delta = move.normalized * distance;
            if (OnLadder && !_down && move.y < 0)
                move.y = 0;
            body.position += move.normalized * distance;
            //print("delta: " + delta.x + " " + delta.y + " distance " + distance + " move " + move.x + ", " + move.y);
        }

        void UpdateJumpState()
        {
            _jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    _jump = true;
                    _stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (_jump && IsGrounded)
            {
                velocity = new Vector2(velocity.x, jumpTakeOffSpeed * Model.jumpModifier);
                _jump = false;
            }
            else if (_stopJump)
            {
                _stopJump = false;
                if (velocity.y > 0)
                {
                    velocity = new Vector2(velocity.x, velocity.y * Model.jumpDeceleration);
                }
            }

            if (_move.x > 0.01f)
                _spriteRenderer.flipX = false;
            else if (_move.x < -0.01f)
                _spriteRenderer.flipX = true;

            _animator.SetBool("grounded", IsGrounded);
            _animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = _move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}