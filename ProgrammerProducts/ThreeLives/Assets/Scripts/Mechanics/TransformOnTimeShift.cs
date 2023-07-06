using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer.Mechanics
{
    [DisallowMultipleComponent]
    public class TransformOnTimeShift : MonoBehaviour
    {
        public bool _holdPlantedObjects = true;
        [Header("GameObject")]
        public GameObject _pastForm;
        public GameObject _currentForm;
        [Header("Event")]
        public UnityEvent _onPast;
        public UnityEvent _onCurrent;
        public bool HoldPlantedObjects => _holdPlantedObjects;
        public bool PreventTransform
        {
            get => _preventTransform;
            set
            {
                if(_preventTransform != value)
                {
                    bool oldValue = _preventTransform;
                    _preventTransform = value;
                    if(value)
                    {
                        foreach(SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
                        {
                            renderer.GetOrAddComponent<FrozenMarker>();
                        }
                    }
                    else
                    {
                        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
                        {
                            if(renderer.TryGetComponent(out FrozenMarker marker))
                            {
                                Destroy(marker);
                            }
                        }
                    }
                    if (oldValue && FreezingTransform) {
                        TransformByTimeline();
                    }
                }
            }
        }
        public bool _preventTransform;
        public bool FreezingTransform { get; private set; }
        private void Start()
        {
            WorldManager.Instance.onShiftTime.AddListener(() => TransformByTimeline());
            TransformByTimeline();
        }
        public void TransformByTimeline()
        {
            if(!gameObject.activeInHierarchy)
                return;
            if (PreventTransform)
            {
                FreezingTransform = !FreezingTransform;
                return;
            }
            FreezingTransform = false;
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
        public void PlantsSetActive(bool cond)
        {
            foreach (PlantableObject plant in GetComponentsInChildren<PlantableObject>())
            {
                plant.gameObject.SetActive(cond);
            }
        }
    }
}