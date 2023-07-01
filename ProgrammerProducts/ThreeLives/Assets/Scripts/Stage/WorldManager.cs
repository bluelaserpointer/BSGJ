using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using Platformer.UI;
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
    [SerializeField]
    GameUI _gameUI;
    [SerializeField]
    TimeShiftTransition _timeShiftTransition;

    [Header("SE")]
    [SerializeField]
    AudioClip _timeshiftSE;

    public readonly UnityEvent
        onShiftTime = new UnityEvent(),
        onShiftPastTime = new UnityEvent(),
        onShiftCurrentTime = new UnityEvent();

    public static WorldManager Instance { get; private set; }
    public GameUI GameUI => _gameUI;
    public Timeline Timeline { get; private set; }

    private void Awake()
    {
        Instance = this;
        SetTimeline(defaultTimeline, true);
    }
    private void Update()
    {
    }
    public void SetTimeline(Timeline timeline, bool initalizing = false)
    {
        if (!initalizing)
        {
            PlayOneShotSound(_timeshiftSE);
            _timeShiftTransition.ToTransition();
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
        SceneTransition.Instance.LoadNextScene(sceneName);
    }
    public void PlayOneShotSound(AudioClip clip)
    {
        PlayerController.Instance.AudioSource.PlayOneShot(clip);
    }
}
