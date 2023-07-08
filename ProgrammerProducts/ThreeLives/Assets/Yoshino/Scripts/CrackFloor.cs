using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackFloor : MonoBehaviour
{
    //[SerializeField] private bool button=false;
    //崩落までの時間
    [SerializeField] private float crackTime = 0.5f;
    //目標の角度
    [SerializeField] private float targetAngle = -45.0f;
    //private Rigidbody2D rb;
    //アニメーションが終了したか
    private bool isCracked = false;
    //アニメーション中か
    private bool isPlaying = false;
    //経過時間
    private float elapsedTime = 0.0f;
   
    public void SetMoveButton()
    {
        //１度ボタンが押されたら強制的に動かす
        if (isCracked) return;
        //経過時間をリセット、フラグをtrueにする
        isCracked = true;
        isPlaying = true;
        elapsedTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying) UpdateCrackAnim();
    }

    void UpdateCrackAnim()
    {
        if (elapsedTime < crackTime)
        {
            //float alpha = elapsedTime / crackTime;
            //イージングで角度を補間する
            float alpha = QuartIn(elapsedTime, crackTime, 0.0f, 1.0f);
            float angle = Mathf.Lerp(0.0f, targetAngle, alpha);
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            //経過時間を計算
            elapsedTime += Time.deltaTime;
        }
        else
        {
            //経過時間を過ぎれば目標の角度にし、アニメーションを終了する
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, targetAngle);
            isPlaying = false;
        }
    }
    public float QuartIn(float t, float totaltime, float min, float max)
    {
        max -= min;
        t /= totaltime;
        return max * t * t * t * t + min;
    }
}