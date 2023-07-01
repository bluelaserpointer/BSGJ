using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class PlantableObject : MonoBehaviour
{
    [SerializeField]
    TransformOnTimeShift _transformer;

    private void Start()
    {
        if(_transformer != null && WorldManager.Instance.Timeline == Timeline.Current)
        {
            _transformer._currentForm.SetActive(false);
            _transformer._currentForm = _transformer._pastForm;
            _transformer._pastForm = null;
        }
    }
    public virtual void PrepareDestroy()
    {
    }
    public void Destroy()
    {
        PrepareDestroy();
        Destroy(gameObject);
    }
}
