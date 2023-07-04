using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class FrozenMarker : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color originalColor;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        spriteRenderer.color *= Color.cyan;
    }
    private void OnDestroy()
    {
        spriteRenderer.color = originalColor;
    }
}
