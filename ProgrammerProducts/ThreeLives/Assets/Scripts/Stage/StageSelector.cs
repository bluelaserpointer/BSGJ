using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelector : MonoBehaviour
{
    [SerializeField] StageInfo[] stageInfoList;
    [SerializeField] int firstSelectIndex;
    [SerializeField] SpriteRenderer spriteImage;
    [SerializeField] Text stageNameText;

    int selectIndex;
    StageInfo SelectingStage => stageInfoList[selectIndex];

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            SetStage(selectIndex = firstSelectIndex);
        }
        catch (System.Exception e)
        {
            WorldManager.Instance.DebugText.text = e.Message;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SetStage(int stageNumber)
    {
        selectIndex = stageNumber;
        spriteImage.sprite = SelectingStage.PreviewImage;
        stageNameText.text = SelectingStage.DisplayName;
    }

    public void ShowNextStage()
    {
        // Debug.Log("next");
        SetStage((selectIndex + 1 + stageInfoList.Length) % stageInfoList.Length);
    }
    public void ShowPreviousStage()
    {
        // Debug.Log("previous");
        SetStage((selectIndex - 1 + stageInfoList.Length) % stageInfoList.Length);

    }

    public void GoToNextStage()
    {
        SelectingStage.LoadScene();
    }

}
