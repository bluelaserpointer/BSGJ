using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlantRetriever : MonoBehaviour
{
    [SerializeField]
    float chasePlayerVelocity = 5;
    [SerializeField]
    List<Color> plantColors;

    public int PlantID { get; private set; }

    new SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
    public void Init(int plantID)
    {
        PlantID = plantID;
        renderer.color = plantColors[plantID];
    }
    void Update()
    {
        Vector3 playerPos = PlayerController.Instance.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, playerPos, chasePlayerVelocity * Time.deltaTime);
        if(Vector3.Distance(transform.position, playerPos) < 1)
        {
            Destroy(gameObject);
            PlayerController.Instance.OnRetrievedPlant(PlantID);
        }
    }
}
