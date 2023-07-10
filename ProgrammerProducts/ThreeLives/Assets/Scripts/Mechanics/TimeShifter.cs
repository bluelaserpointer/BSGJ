using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TimeShifter : Interactable
{
    protected override void Awake()
    {
        base.Awake();
        OnInteract.AddListener(() =>
        {
            if(!TimeShiftTransition.instance.isMoving){
                WorldManager.Instance.SetTimeline(WorldManager.Instance.Timeline == Timeline.Current ? Timeline.Past : Timeline.Current);
            }
        });
    }
}
