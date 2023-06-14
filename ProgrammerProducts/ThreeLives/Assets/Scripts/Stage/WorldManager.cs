using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum Timeline { Past, Current }

[DisallowMultipleComponent]
public class WorldManager : MonoBehaviour
{
    //inspector
    [SerializeField]
    Color _pastBgColor, _currentBgColor;
    [SerializeField]
    Camera _camera;
    [SerializeField]
    Timeline defaultTimeline = Timeline.Past;

    [Header("SE")]
    [SerializeField]
    AudioClip _timeshiftSE;

    public readonly UnityEvent
        onShiftTime = new UnityEvent(),
        onShiftPastTime = new UnityEvent(),
        onShiftCurrentTime = new UnityEvent();

    public static WorldManager Instannce { get; private set; }
    public Timeline Timeline { get; private set; }

    PlatformerModel _platformerModel = Simulation.GetModel<PlatformerModel>();
    public static PlayerController PlayerController => Instannce._platformerModel.player;

    private void Awake()
    {
        Instannce = this;
        SetTimeline(defaultTimeline, true);
    }
    public void SetTimeline(Timeline timeline, bool initalizing = false)
    {
        if (!initalizing)
        {
            PlayOneShotSound(_timeshiftSE);
        }
        switch (Timeline = timeline)
        {
            case Timeline.Past:
                onShiftPastTime.Invoke();
                _camera.backgroundColor = _pastBgColor;
                break;
            case Timeline.Current:
                onShiftCurrentTime.Invoke();
                _camera.backgroundColor = _currentBgColor;
                break;
        }
        onShiftTime.Invoke();
    }
    public void LoadNewScene(string sceneName)
    {
        if (sceneName == "")
            return;
        SceneManager.LoadScene(sceneName);
    }
    public void PlayOneShotSound(AudioClip clip)
    {
        PlayerController.AudioSource.PlayOneShot(clip);
    }
}
