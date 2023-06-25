using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.View
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRoller : MonoBehaviour
    {
        public Vector2 Speed;

        private SpriteRenderer _renderer;

        void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            _renderer.material.mainTextureOffset = Speed * Time.timeSinceLevelLoad;
        }
    }
}