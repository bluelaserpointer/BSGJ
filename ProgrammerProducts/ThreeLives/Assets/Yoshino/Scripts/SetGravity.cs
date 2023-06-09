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
        //��������̍������v�Z
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
        //�w��̍����܂ŏd�͂�^����
        if (transform.position.y / finalHeight >= 1)     
        {
            rb.AddForce(gravity);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    
    //�g�O�����̃{�^��
    public void SetGravityButton()
    {
        button = button ? button = false : button = true;
    }
    
}