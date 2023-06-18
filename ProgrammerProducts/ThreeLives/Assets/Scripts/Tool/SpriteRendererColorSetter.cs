using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererColorSetter : MonoBehaviour
{
    SpriteRenderer _renderer;

    public List<Color> colors = new List<Color>();

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor(int colorIndex) //color parameter not visible from unity event inspector
    {
        _renderer.color = colors[colorIndex];
    }
}
