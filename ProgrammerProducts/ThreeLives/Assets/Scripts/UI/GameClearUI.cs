using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearUI : MonoBehaviour
{
    [SerializeField]
    CanvasGroupFader cut1, cut2;
    [SerializeField]
    CanvasGroupFader transparentBlack;

    int state = 0;
    float lastCutStartTime = -1;
    public void NextCut()
    {
        switch(++state)
        {
            case 1:
                cut1.SetTargetAlpha(0);
                cut2.SetTargetAlpha(1);
                break;
            case 2:
                cut2.SetTargetAlpha(0);
                transparentBlack.SetTargetAlpha(0);
                lastCutStartTime = Time.timeSinceLevelLoad;
                break;
        }
    }
    private void Awake()
    {
        cut1.GetComponent<CanvasGroup>().alpha = 0;
        cut2.GetComponent<CanvasGroup>().alpha = 0;
    }
    private void Start()
    {
        cut1.SetTargetAlpha(1);
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            NextCut();
        }
        if (lastCutStartTime != -1 && Time.timeSinceLevelLoad - lastCutStartTime > 1)
        {
            SceneManager.LoadScene("Title");
        }
    }

}
