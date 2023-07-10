using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenFloor : MonoBehaviour
{
    [SerializeField] private float durability = 3;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody.velocity.magnitude >= durability)   
        {
            Destroy(this.gameObject);
        }
    }
}
