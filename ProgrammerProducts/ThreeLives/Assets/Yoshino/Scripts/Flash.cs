using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    //オン状態の画像
    [SerializeField] private Sprite On;
    //オフ状態の画像
    [SerializeField] private Sprite Off;
    //プッシュ状態の画像
    [SerializeField] private Sprite Pushing;
    //プッシュ画像を表示する時間
    [SerializeField] private float pushingTime = 0.5f;
    //ボタンの状態
    [SerializeField] private bool button;
    //一度だけ押せるようにする
    [SerializeField] private bool keepPush;
    //スプライトレンダー
    private SpriteRenderer sr;
    //経過時間
    private float elapsedTime = 0.0f;
    //プッシュ中
    private bool isPushing = false;
    //keepPushがtrueの時に、ボタンが押されたか
    private bool isPushed = false;
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        if (elapsedTime < pushingTime)
        {
            //プッシュ中の画像に切り替え
            sr.sprite = Pushing;
            isPushing = true;
            //経過時間を計算
            elapsedTime += Time.deltaTime;
            return;
        }

        if (button)
        {
            sr.sprite = On;
        }
        else
        {
            sr.sprite = Off;
        }
        isPushing = false;

    }
    public void Flashing()
    {
        //一度だけ押されるボタン
        if (keepPush)
        {
            if (isPushed) return;
            isPushed = true;
        }
        if (isPushing)
            return;
        //トグル式のボタン
        button = button ? button = false : button = true;
        //経過時間をリセット
        elapsedTime = 0.0f;
    }
}
