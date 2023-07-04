using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField]
    string _nextScene;
    void Update()
    {
        if(Input.anyKey)
        {
            SceneTransition.Instance.LoadNextScene(_nextScene);
        }
    }
}
