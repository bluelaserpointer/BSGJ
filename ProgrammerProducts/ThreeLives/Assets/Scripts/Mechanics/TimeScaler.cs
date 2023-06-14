using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : Interactable
{
    [SerializeField]
    float _timeScale = 2;

    bool _interacted = false;
    private void Start()
    {
        OnInteractStay.AddListener(() =>
        {
            _interacted = true;
        });
    }
    private void Update()
    {
        Time.timeScale = _interacted ? _timeScale : 1;
    }
    private void LateUpdate()
    {
        _interacted = false;
    }
}
