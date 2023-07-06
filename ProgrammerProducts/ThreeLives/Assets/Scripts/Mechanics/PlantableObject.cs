using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class PlantableObject : MonoBehaviour
{
    [SerializeField]
    Sprite _icon;

    public Sprite Icon => _icon;
    public bool PreventRetrieve { get; protected set; }

    public virtual void PrepareDestroy()
    {
    }
    public void Destroy()
    {
        PrepareDestroy();
        Destroy(gameObject);
    }
}
