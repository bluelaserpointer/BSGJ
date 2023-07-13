using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BrokenFloor : MonoBehaviour
{
    [SerializeField] private AudioClip se;
    [SerializeField] AudioSource source;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            source.PlayOneShot(se);
        }
        if (collision.gameObject.tag == "Block")
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            Destroy(this.gameObject);
        }
    }

}
