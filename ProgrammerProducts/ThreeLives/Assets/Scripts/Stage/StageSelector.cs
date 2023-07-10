using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StageSelector : MonoBehaviour
{
    [System.Serializable]
	struct Stage
	{
        [SerializeField] [Tooltip("読み込むシーン名")]
        public SceneObject sceneName; 
        
        [SerializeField] [Tooltip("読み込むシーンの画像")]
        public Sprite sceneImage; 
        
        [SerializeField] [Tooltip("表示される文字")]
        public string stageName;   
	}
    
    [SerializeField] Stage[] StageList;
    [SerializeField] SpriteRenderer spriteImage;
    [SerializeField] TMP_Text stageNameTMP;

    int nowStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetStage(nowStage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetStage(int stageNumber)
    {
        spriteImage.sprite = StageList[stageNumber].sceneImage;
        stageNameTMP.text = StageList[stageNumber].stageName;

        nowStage = stageNumber;
    }

    public void ShowNextStage()
    {
        // Debug.Log("next");
        SetStage((nowStage + 1 + StageList.Length) % StageList.Length);


    }
    public void ShowPreviousStage()
    {
        // Debug.Log("previous");
        SetStage((nowStage - 1 + StageList.Length) % StageList.Length);

    }

        public void GoToNextStage()
    {
        SceneTransition.Instance.LoadNextScene(StageList[nowStage].sceneName);
    }

}
