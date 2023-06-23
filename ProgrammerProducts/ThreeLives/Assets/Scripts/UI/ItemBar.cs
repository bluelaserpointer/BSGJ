using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ItemBar : MonoBehaviour
{
    [Header("ItemBase")]
    [SerializeField]
    Image _selectHighlight;
    [SerializeField]
    Transform _itemsParent;

    public void OnSelectedItem(int index)
    {
        if (index < 0 || index >= _itemsParent.childCount)
        {
            _selectHighlight.gameObject.SetActive(false);
            return;
        }
        _selectHighlight.gameObject.SetActive(true);
        _selectHighlight.transform.position = _itemsParent.GetChild(index).position;
    }
}
