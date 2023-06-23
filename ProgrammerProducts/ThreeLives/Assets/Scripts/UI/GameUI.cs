using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameUI : MonoBehaviour
{
    [SerializeField]
    ItemBar _itemBar;

    public ItemBar ItemBar => _itemBar;
}
