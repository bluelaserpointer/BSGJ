using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameUI : MonoBehaviour
{
    [SerializeField]
    ItemBar _itemBar;
    [SerializeField]
    Text _itemCollectProgressText;

    public ItemBar ItemBar => _itemBar;
    public int totalItemCount;

    private void Start()
    {
        totalItemCount = Resources.LoadAll<CollectItemIdentifier>("CollectItemIdentifier").Length;
        UpdateUI();
    }
    public void UpdateUI()
    {
        _itemCollectProgressText.text = SaveData.Instance.collectedItems.Count + " / " + totalItemCount;
    }
}
