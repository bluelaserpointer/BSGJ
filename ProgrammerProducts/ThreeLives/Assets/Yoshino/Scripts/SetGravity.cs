using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGravity : MonoBehaviour
{
    [SerializeField] bool button;
    [SerializeField] private float fallLength;
    private float finalHeight;
    private Rigidbody2D rb;
    private Vector2 gravity = new Vector2(0, -9.81f);
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //落ちた後の高さを計算
        finalHeight = transform.position.y + fallLength;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (button)
        {
            FallToTarget();
        }

    }

    private void FallToTarget()
    {
        //指定の高さまで重力を与える
        if (transform.position.y / finalHeight >= 1)     
        {
            rb.AddForce(gravity);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    
    //トグル式のボタン
    public void SetGravityButton()
    {
        button = button ? button = false : button = true;
    }
    
}