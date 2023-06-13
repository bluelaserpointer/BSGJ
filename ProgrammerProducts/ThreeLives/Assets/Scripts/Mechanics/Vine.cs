using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : PlantableObject
{
    [SerializeField]
    Quaternion _fixedAngle = Quaternion.identity;
    private void LateUpdate()
    {
        transform.rotation = _fixedAngle;
    }
}
