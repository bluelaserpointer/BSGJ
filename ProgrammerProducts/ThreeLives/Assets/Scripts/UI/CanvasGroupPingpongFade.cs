using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupPingpongFade : MonoBehaviour
{
    public float alphaChangeSpeed = 1;
    [Range(0, 1)]
    public float minAlpha = 0, maxAlpha = 1;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Update()
    {
        canvasGroup.alpha = minAlpha + (maxAlpha - minAlpha) * Mathf.PingPong(Time.timeSinceLevelLoad * alphaChangeSpeed, 1);
    }
}
