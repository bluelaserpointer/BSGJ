using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Sprite open;
    private Collider2D col2;
    private SpriteRenderer sr;

    public void OpenDoor()
    {
        sr.sprite = open;
        Destroy(col2);
    }
    // Start is called before the first frame update
    void Start()
    {
        col2 = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
