using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class PlantableObject : MonoBehaviour
{
    [SerializeField]
    TransformOnTimeShift _transformer;

    private void Awake()
    {
        if(WorldManager.Instance.Timeline == Timeline.Current)
        {
            _transformer._currentForm.SetActive(false);
            _transformer._currentForm = _transformer._pastForm;
            _transformer._pastForm = null;
        }
    }
}
