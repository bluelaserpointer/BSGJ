using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CollectableItem : MonoBehaviour
{
    [SerializeField]
    CollectItemIdentifier _identifier;
    [SerializeField]
    float _takenColorValue = 0.12F;

    private void Start()
    {
        if(SaveData.Instance.collectedItems.Contains(_identifier))
        {
            foreach(SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>()) {
                renderer.color = new Color(1, 1, 1, _takenColorValue);
            }
            GetComponentInChildren<Collider2D>().enabled = false;
        }
    }
    public void OnCollected()
    {
        SaveData.Instance.CollectItem(_identifier);
    }
}
