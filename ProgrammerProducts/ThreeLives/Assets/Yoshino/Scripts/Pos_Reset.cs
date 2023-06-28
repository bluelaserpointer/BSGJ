using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Pos_Reset : MonoBehaviour
{
    private Vector3 pastPos;
    private Rigidbody2D rb;
    public void PosReset()
    {
        rb.velocity = Vector2.zero;
        switch (WorldManager.Instance.Timeline)
        {
            case Timeline.Past:
                //transform.position = pastPos;
                pastPos = transform.position;
                
                break;
            case Timeline.Current:
                transform.position = pastPos;
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pastPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }
}