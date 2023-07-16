using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SaveData : MonoBehaviour
{
    public static SaveData Instance { get; private set; }
    public List<CollectItemIdentifier> collectedItems = new List<CollectItemIdentifier>();
    [HideInInspector]
    public float bgmPlaybackTime;

    public void Init()
    {
        Instance = this;
        bgmPlaybackTime = 0;
    }
    public void CollectItem(CollectItemIdentifier itemIdentifier)
    {
        if(collectedItems.Contains(itemIdentifier))
        {
            print("<!> duplicated item collection");
            return;
        }
        collectedItems.Add(itemIdentifier);
        WorldManager.Instance.GameUI.UpdateUI();
        if (collectedItems.Count == WorldManager.Instance.GameUI.totalItemCount)
        {
            WorldManager.Instance.GameClear();
            collectedItems.Clear();
        }
    }
}
