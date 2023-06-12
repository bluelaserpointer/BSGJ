using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer.Mechanics
{
    [DisallowMultipleComponent]
    public class TransformOnTimeShift : MonoBehaviour
    {
        [SerializeField]
        GameObject _pastForm, _currentForm;

        private void Start()
        {
            WorldManager.Instannce.onShiftTime.AddListener(() => TransformByTimeline());
            TransformByTimeline();
        }
        public void TransformByTimeline()
        {
            switch (WorldManager.Instannce.Timeline)
            {
                case Timeline.Past:
                    _pastForm?.SetActive(true);
                    _currentForm?.SetActive(false);
                    break;
                case Timeline.Current:
                    _pastForm?.SetActive(false);
                    _currentForm?.SetActive(true);
                    break;
            }
        }
    }

}