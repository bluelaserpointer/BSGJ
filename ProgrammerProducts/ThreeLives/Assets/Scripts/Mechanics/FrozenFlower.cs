using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class FrozenFlower : PlantableObject
{
    [SerializeField]
    float radius;
    Transform _copiesHierarchyRoot;
    private void Start()
    {
        _copiesHierarchyRoot = new GameObject("FrozenCopy").transform;
        List<Collider2D> colliders = new List<Collider2D>();
        List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        foreach(Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
        {
            TransformOnTimeShift transformer = collider.GetComponentInParent<TransformOnTimeShift>();
            if (transformer != null)
            {
                colliders.Add(collider);
                foreach (SpriteRenderer renderer in transformer.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (!renderers.Contains(renderer) && renderer.GetComponentInParent<FrozenFlower>() == null)
                        renderers.Add(renderer);
                }
            }
        }
        colliders.ForEach(collider => {
            GameObject copy = new GameObject(collider.name);
            copy.transform.SetParent(_copiesHierarchyRoot, true);
            copy.transform.position = collider.transform.position;
            copy.transform.rotation = collider.transform.rotation;
            if (collider.TryGetComponent(out Wall wall))
            {
                copy.AddComponent<Wall>().penetratable = wall.penetratable;
            }
            if (collider.TryGetComponent(out LadderZone ladderZone))
            {
                copy.AddComponent<LadderZone>();
            }
            Collider2D copyCollider = null;
            if(collider.GetType() == typeof(BoxCollider2D))
            {
                BoxCollider2D boxCopy = copy.AddComponent<BoxCollider2D>();
                if(!((BoxCollider2D)collider).autoTiling)
                {
                    boxCopy.size = ((BoxCollider2D)collider).size;
                    copy.transform.localScale = collider.transform.lossyScale;
                }
                else
                {
                    //TODO: rotated objects
                    Bounds bounds = collider.bounds;
                    boxCopy.size = bounds.size;
                }
                boxCopy.offset = ((BoxCollider2D)collider).offset;
                copyCollider = boxCopy;
            }
            else if(collider.GetType() == typeof(PolygonCollider2D))
            {
                PolygonCollider2D polygonCopy = copy.AddComponent<PolygonCollider2D>();
                polygonCopy.points = ((PolygonCollider2D)collider).points;
                copyCollider = polygonCopy;
            }
            else
            {
                print("<!> Unexpected collider type");
            }
            if(copyCollider != null)
            {
                copyCollider.isTrigger = collider.isTrigger;
            }
        });
        renderers.ForEach(renderer => {
            SpriteRenderer rendererCopy = new GameObject(renderer.name).AddComponent<SpriteRenderer>();
            rendererCopy.transform.SetParent(_copiesHierarchyRoot);
            rendererCopy.transform.position = renderer.transform.position + new Vector3(1, -1, 0) * 0.01F;
            rendererCopy.transform.rotation = renderer.transform.rotation;
            rendererCopy.transform.localScale = renderer.transform.lossyScale;
            rendererCopy.sprite = renderer.sprite;
            rendererCopy.sortingOrder = renderer.sortingOrder + 1;
            rendererCopy.drawMode = renderer.drawMode;
            rendererCopy.size = renderer.size;
            Color newColor = rendererCopy.color * Color.cyan;
            newColor.a *= 0.75F;
            rendererCopy.color = newColor;
            });
    }
    private void OnDestroy()
    {
        if (_copiesHierarchyRoot != null)
            Destroy(_copiesHierarchyRoot.gameObject);
    }
}
