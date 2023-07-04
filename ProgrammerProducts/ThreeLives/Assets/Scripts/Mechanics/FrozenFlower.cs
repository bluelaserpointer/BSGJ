using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class FrozenFlower : PlantableObject
{
    [SerializeField]
    float radius;
    [SerializeField]
    List<TransformOnTimeShift> transformers = new List<TransformOnTimeShift>();
    private void Start()
    {
        foreach(Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
        {
            foreach(TransformOnTimeShift transformer in collider.GetComponentsInParent<TransformOnTimeShift>())
            {
                if (transformer != null && !transformers.Contains(transformer))
                {
                    transformers.Add(transformer);
                }
            }
        }
        FreezeTransform(true);
    }
    public void FreezeTransform(bool cond)
    {
        new List<TransformOnTimeShift>(transformers).ForEach(transformer => {
            if (transformer != null)
                transformer.PreventTransform = cond;
            else
                transformers.Remove(transformer);
            });
    }
    public override void PrepareDestroy()
    {
        FreezeTransform(false);
    }
}
