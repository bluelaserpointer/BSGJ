using UnityEngine;
using Platformer.Core;
using Platformer.Model;
using UnityEngine.Events;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Marks a trigger as a VictoryZone, usually used to end the current game level.
    /// </summary>
    [DisallowMultipleComponent]
    public class Interactable : MonoBehaviour
    {
        [SerializeField]
        GameObject _stepInActiveObj;
        public UnityEvent OnInteract, OnInteractStay, OnStepIn, OnStepOut;

        PlayerController Player => PlayerController.Instance;
        protected virtual void Awake()
        {
            if (_stepInActiveObj != null)
                _stepInActiveObj.SetActive(false);
        }
        void OnTriggerEnter2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                if(_stepInActiveObj != null)
                    _stepInActiveObj.SetActive(true);
                Player.avaliableInteractable = this;
                OnStepIn.Invoke();
            }
        }
        void OnTriggerExit2D(Collider2D collider)
        {
            var p = collider.gameObject.GetComponent<PlayerController>();
            if (p != null)
            {
                if (_stepInActiveObj != null)
                    _stepInActiveObj.SetActive(false);
                Player.avaliableInteractable = null;
                OnStepOut.Invoke();
            }
        }
    }
}