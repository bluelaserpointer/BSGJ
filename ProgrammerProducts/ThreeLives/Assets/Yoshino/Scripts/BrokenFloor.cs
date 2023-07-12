using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrokenFloor : MonoBehaviour
{
    [SerializeField] private float durability = 3;
    [SerializeField] private AudioClip se;
    [SerializeField] AudioSource source;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            source.PlayOneShot(se);
        }

        if (collision.rigidbody.velocity.magnitude >= durability)   
        {
            Destroy(this.gameObject);
        }
    }
}
