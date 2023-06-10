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

    public static WorldManager Instannce { get; private set; }

    private void Awake()
    {
        Instannce = this;
        SetTimeline(0);
    }
    public void SetTimeline(int timeline)
    {
        switch(timeline)
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
}
