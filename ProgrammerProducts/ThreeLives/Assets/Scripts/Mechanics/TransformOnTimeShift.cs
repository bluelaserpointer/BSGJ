using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer.Mechanics
{
    [DisallowMultipleComponent]
    public class TransformOnTimeShift : MonoBehaviour
    {
        public bool _holdPlantedObjects = true;
        public GameObject _pastForm, _currentForm;
        public UnityEvent _onPast, _onCurrent;

        public bool HoldPlantedObjects => _holdPlantedObjects;
        private void Start()
        {
            WorldManager.Instance.onShiftTime.AddListener(() => TransformByTimeline());
            TransformByTimeline();
        }
        public void TransformByTimeline()
        {
            if(!gameObject.activeInHierarchy)
                return;
            switch (WorldManager.Instance.Timeline)
            {
                case Timeline.Past:
                    if(_pastForm != null)
                        _pastForm.SetActive(true);
                    if (_currentForm != null)
                        _currentForm.SetActive(false);
                    _onPast.Invoke();
                    break;
                case Timeline.Current:
                    if (_pastForm != null)
                        _pastForm.SetActive(false);
                    if (_currentForm != null)
                        _currentForm.SetActive(true);
                    _onCurrent.Invoke();
                    break;
            }
        }
    }

}