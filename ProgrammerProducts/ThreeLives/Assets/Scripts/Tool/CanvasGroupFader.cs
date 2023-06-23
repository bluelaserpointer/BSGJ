using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFader : MonoBehaviour
{
    public float targetAlpha = 1;
    public float fadeSpeed = 1;

    CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = targetAlpha;
    }
    private void Update()
    {
        float alpha = _canvasGroup.alpha;
        alpha = Mathf.MoveTowards(alpha, targetAlpha, fadeSpeed * Time.deltaTime);
        _canvasGroup.alpha = alpha;
    }
    public void SetTargetAlpha(float value)
    {
        targetAlpha = value;
    }
}
