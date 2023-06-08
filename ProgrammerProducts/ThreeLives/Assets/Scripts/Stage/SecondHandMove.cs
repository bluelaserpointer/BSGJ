using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondHandMove : MonoBehaviour
{
    float second = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        second += Time.deltaTime;
        this.gameObject.transform.eulerAngles = new Vector3(0, 0, -second * 6);
    }
}
