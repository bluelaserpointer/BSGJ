using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantedVine : PlantableObject
{
    [SerializeField]
    TransformOnTimeShift _transformer;

    private void Start()
    {
        if (WorldManager.Instance.Timeline == Timeline.Current)
        {
            _transformer._currentForm.SetActive(false);
            _transformer._currentForm = _transformer._pastForm;
            _transformer._pastForm = null;
            WorldManager.Instance.onShiftTime.AddListener(UpdateRetrieveState);
        }
    }
    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }
    private void UpdateRetrieveState()
    {
        if (WorldManager.Instance.Timeline == Timeline.Past)
        {
            PreventRetrieve = true;
        }
        else
        {
            PreventRetrieve = false;
        }
    }
    private void OnDestroy()
    {
        /*
        if(WorldManager.Instance != null)
            WorldManager.Instance.onShiftTime.RemoveListener(UpdateRetrieveState);
        */
    }
}
