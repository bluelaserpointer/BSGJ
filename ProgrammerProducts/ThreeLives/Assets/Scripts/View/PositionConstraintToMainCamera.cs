using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PositionConstraintToMainCamera : MonoBehaviour
{
    public bool constraintX, constraintY, constraintZ;
    public new Camera camera;

    Vector3 offset;

    private void Awake()
    {
        camera = Camera.main;
        offset = camera.transform.position - transform.position;
    }
    private void LateUpdate()
    {
        transform.position = camera.transform.position + offset;
    }
}
