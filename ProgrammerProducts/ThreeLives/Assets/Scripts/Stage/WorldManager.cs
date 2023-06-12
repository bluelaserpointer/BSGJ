using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class WorldManager : MonoBehaviour
{
    //inspector
    [SerializeField]
    Color _pastBgColor, _currentBgColor;
    [SerializeField]
    Camera _camera;
    [SerializeField]
    int defaultTimeline = 0;

    public static WorldManager Instannce { get; private set; }

    PlatformerModel _platformerModel = Simulation.GetModel<PlatformerModel>();
    public static PlayerController PlayerController => Instannce._platformerModel.player;

    private void Awake()
    {
        Instannce = this;
        SetTimeline(defaultTimeline);
    }
    public void SetTimeline(int timeline)
    {
        switch (timeline)
        {
            case 0:
                _camera.backgroundColor = _pastBgColor;
                break;
            case 1:
                _camera.backgroundColor = _currentBgColor;
                break;
        }
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
