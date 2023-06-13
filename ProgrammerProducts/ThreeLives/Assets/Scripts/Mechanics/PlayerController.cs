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
        //Inspector
        [Header("SE")]
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        [Header("Ability")]
        [SerializeField]
        Vine _vineSeedPrefab;
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

        private bool _stopJump;
        bool _jump;
        bool _down;
        Vector2 _move;
        SpriteRenderer _spriteRenderer;
        internal Animator _animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        [HideInInspector]
        public Interactable avaliableInteractable;

        [HideInInspector]
        public int _onLadderCount;
        public bool OnLadder => _onLadderCount > 0;
        List<Collider2D> _penetratingColliders = new List<Collider2D>();

        List<PlantableObject> _plantedObjects = new List<PlantableObject>();


        void Awake()
        {
            Health = GetComponent<Health>();
            AudioSource = GetComponent<AudioSource>();
            Collider2d = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
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
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if(!GroundCollider)
                {
                    //cannot plant in this position
                }
                else if (WorldManager.Instannce.Timeline == Timeline.Current)
                {
                    //prevent plant action in current time
                }
                else
                {
                    Transform seedTarget;
                    TransformOnTimeShift parentTransformNode = GroundCollider.GetComponentInParent<TransformOnTimeShift>();
                    if(parentTransformNode != null)
                    {
                        seedTarget = parentTransformNode.transform;
                    }
                    else
                    {
                        seedTarget = GroundCollider.transform;
                    }
                    Vine vine = Instantiate(_vineSeedPrefab, seedTarget);
                    vine.transform.position = transform.position;
                    _plantedObjects.Add(vine);
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
                if(!Collider2d.Distance(penetratingCollider).isOverlapped)
                {
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
            }
            else
            {
                IsGrounded = false;
                GroundCollider = null;
                //if already falling, fall faster than the jump speed, otherwise use normal gravity.
                if (velocity.y < 0)
                {
                    velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
                    if(OnLadder && _down && velocity.y < -_maxClimbDownSpeed)
                    {
                        velocity.y = -_maxClimbDownSpeed;
                    }
                }
                else
                    velocity += Physics2D.gravity * Time.deltaTime;
            }

            velocity.x = targetVelocity.x;


            var deltaPosition = velocity * Time.deltaTime;

            var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

            var xmove = moveAlongGround * deltaPosition.x;
            PerformMovement(xmove, false);


            if (!IsGrounded || velocity.y > 0)
            {
                Vector2 ymove = Vector2.up * deltaPosition.y;
                if (OnLadder)
                {
                    body.position += ymove;
                }
                else
                {
                    PerformMovement(ymove, true);
                }
            }
        }

        public void RemoveAllPlantedObjects()
        {
            foreach(var plant in _plantedObjects)
            {
                Destroy(plant.gameObject);
            }
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
                    if (hitInfo.collider.GetType() == typeof(EdgeCollider2D))
                    {   
                        if (
                            move.y >= 0 //penetrate edge collider ceilings while jumping
                            || OnLadder && _down //penetrate edge coolider grounds while climb down ladders
                            )
                        {
                            //remember those colliders should be ignored while overlapping
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
                    else
                    {
                        //print(currentNormal.y + " smaller " + minGroundNormalY);
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
                        velocity.x *= 0;
                        velocity.y = Mathf.Min(velocity.y, 0);
                    }
                    //remove shellDistance from actual move distance.
                    var modifiedDistance = hitInfo.distance - shellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }
            }
            body.position += move.normalized * distance;
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
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                _jump = false;
            }
            else if (_stopJump)
            {
                _stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
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