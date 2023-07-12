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

    [Header("BGM")]
    [SerializeField]
    AudioSource _bgmAudioSource;
    [SerializeField]
    AudioClip _currentSE, _pastSE;

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
        if (_bgmAudioSource.clip != null)
        {
            SaveData.Instance.bgmPlaybackTime = _bgmAudioSource.time;
        }
        switch (Timeline = timeline)
        {
            case Timeline.Past:
                onShiftPastTime.Invoke();
                _camera.backgroundColor = _pastBgColor;
                _bgmAudioSource.clip = _pastSE;
                break;
            case Timeline.Current:
                onShiftCurrentTime.Invoke();
                _camera.backgroundColor = _currentBgColor;
                _bgmAudioSource.clip = _currentSE;
                break;
        }
        _bgmAudioSource.time = SaveData.Instance.bgmPlaybackTime;
        _bgmAudioSource.Play();
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
