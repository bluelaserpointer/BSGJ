using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class DontDestroyOnLoad : MonoBehaviour
{
    private static bool isFirstInit = true;
    [SerializeField]
    UnityEvent onFirstInit;
    public UnityEvent OnFirstInit => onFirstInit;
    void Awake()
    {
        if (isFirstInit)
        {
            isFirstInit = false;
            gameObject.transform.parent = null;
            DontDestroyOnLoad(gameObject);
            OnFirstInit.Invoke();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
