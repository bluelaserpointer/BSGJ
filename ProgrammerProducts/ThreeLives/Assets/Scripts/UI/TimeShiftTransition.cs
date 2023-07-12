using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Platformer.Model;
using Platformer.Core;

public class TimeShiftTransition : MonoBehaviour
{
    [SerializeField] float maskSpeed = 1;
    [SerializeField] Camera cam;
    [SerializeField] GameObject pastMask;
    [SerializeField] GameObject currentMask;
    [SerializeField] GameObject maskEndPoint;

    public static TimeShiftTransition instance;

    Vector3 imgPos;
    Image screenImage;
    GameObject mask;
    Vector3 startMaskPos;
    Vector3 endMaskPos;
    float maskDistance;
    float startTime;
    public bool isMoving = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update() // ワイプ動作
    {
        if (isMoving)
        {
            float t = ((Time.time - startTime) * maskSpeed * Screen.width) / maskDistance;
            mask.transform.position = Vector3.Lerp(startMaskPos, endMaskPos, t);
            if (t >= 1)
            {
                isMoving = false;
                mask.transform.position = startMaskPos;
                screenImage.enabled = false;
            }
            screenImage.transform.position = imgPos;
        }

    }

    public void ToTransition() // ワイプ設定 
    {
        if (!isMoving)
        {
            switch (WorldManager.Instance.Timeline)
            {
                case Timeline.Past:
                    mask = pastMask;
                    startMaskPos = mask.transform.position;

                    endMaskPos = new Vector3(startMaskPos.x + maskEndPoint.transform.position.x, startMaskPos.y, startMaskPos.z);
                    screenImage = pastMask.transform.GetChild(0).GetComponent<Image>();

                    break;
                case Timeline.Current:
                    mask = currentMask;
                    startMaskPos = mask.transform.position;

                    endMaskPos = new Vector3(startMaskPos.x - maskEndPoint.transform.position.x, startMaskPos.y, startMaskPos.z);
                    screenImage = currentMask.transform.GetChild(0).GetComponent<Image>();
                    break;
            }
            screenImage.enabled = true;
            imgPos = screenImage.transform.position;
            screenImage.sprite = GetScreenshot();
            maskDistance = Vector3.Distance(startMaskPos, endMaskPos);
            startTime = Time.time;

            isMoving = true;
        }
    }

    Sprite GetScreenshot() // 現在のカメラの映像をspriteとして取得
    {
        var rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
        rt.Create();

        Texture2D texture2D = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, false);
        cam.targetTexture = rt;
        cam.Render();

        // RenderTextureをtexture2Dに変換
        RenderTexture.active = rt;
        texture2D.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture2D.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;

        // texture2ｄをspriteに変換
        Sprite spriteimage = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);

        return spriteimage;
    }
}
