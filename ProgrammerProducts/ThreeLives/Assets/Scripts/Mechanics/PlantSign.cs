using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlantSign : MonoBehaviour
{
    public PlantableObject Plant { get; private set; }
    [SerializeField]
    GameObject retrieveableState, notRetrieveableState;
    [SerializeField]
    SpriteRenderer plantIcon;

    public bool Retrieveable => Plant.gameObject.activeInHierarchy && !Plant.PreventRetrieve;

    void Update()
    {
        bool cond = Retrieveable;
        retrieveableState.SetActive(cond);
        notRetrieveableState.SetActive(!cond);
    }
    public void SetPlant(PlantableObject plant)
    {
        Plant = plant;
        plantIcon.sprite = plant.Icon;
    }
}
