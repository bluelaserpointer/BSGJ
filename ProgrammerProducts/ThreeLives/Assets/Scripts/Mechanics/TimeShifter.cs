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
            WorldManager.Instannce.SetTimeline(WorldManager.Instannce.Timeline == Timeline.Current ? Timeline.Past : Timeline.Current);
        });
    }
}
