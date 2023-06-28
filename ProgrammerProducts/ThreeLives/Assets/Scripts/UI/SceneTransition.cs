using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] float transitionSpeed = 1;
    [SerializeField] Image image;

    bool isMoving = false;
    float startTime;
    Color startColor;
    Color endColor;
    string loadSceneName = "";

    public static SceneTransition Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        FadeIn();
        startTime = Time.unscaledTime;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            float t = ((Time.unscaledTime - startTime) * transitionSpeed);
            Debug.Log(t);
            image.color = Color.Lerp(startColor, endColor, t);
            if (t >= 1)
            {
                isMoving = false;
                if (loadSceneName != "")
                {
                    SceneManager.LoadScene(loadSceneName);
                }
            }
        }
    }
    public void LoadNextScene(string nextSceneName)
    {
        FadeOut();
        startTime = Time.unscaledTime;
        loadSceneName = nextSceneName;
        isMoving = true;
    }
    public void LoadNowScene()
    {
        FadeOut();
        startTime = Time.unscaledTime;
        loadSceneName = SceneManager.GetActiveScene().name;
        isMoving = true;
    }

    void FadeOut()
    {
        startColor = new Color(0f, 0f, 0f, 0f);
        endColor = new Color(0f, 0f, 0f, 1f);
    }
    void FadeIn()
    {
        startColor = new Color(0f, 0f, 0f, 1f);
        endColor = new Color(0f, 0f, 0f, 0f);
    }

}
