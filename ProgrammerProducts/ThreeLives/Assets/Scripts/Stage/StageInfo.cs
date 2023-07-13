using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "StageInfo", fileName = "StageInfo")]
public class StageInfo : ScriptableObject
{
    [SerializeField]
    SceneObject scene;
    [SerializeField]
    Sprite previewImage;
    [SerializeField]
    string nameJP, nameCN;

    public string DisplayName => (TranslatableSentence.currentLanguage == Language.Japanese) || (TranslatableSentence.currentLanguage == Language.JapanesePad) ? nameJP : nameCN;
    public Sprite PreviewImage => previewImage;
    public string LoadName => scene;
    public void LoadScene()
    {
        SceneManager.LoadScene(scene);
    }
}
