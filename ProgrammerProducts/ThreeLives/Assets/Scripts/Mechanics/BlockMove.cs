using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Platformer.Core;
using Platformer.Model;

public class BlockMove : MonoBehaviour
{
    public UnityEvent switchClock;
    [SerializeField]
    GameObject parentBlock;
    GameObject player;
    [SerializeField] GameObject actionPoint;
    bool isMoving = false;
    Vector3 offsetPos;
    Vector3 defaultPos;
    [SerializeField]
    float limitedSpeed = 1;
    float tmpSpeed;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        defaultPos = transform.position;
    }
    private void Update()
    {
        if (isMoving)
        {
            // ブロックの当たり判定が邪魔して押し進めないのでcolliderを消す もうちょっといい処理あるかも
            parentBlock.GetComponent<BoxCollider2D>().enabled = false;

            // ジャンプすると解除
            if (0.1 < offsetPos.y - transform.position.y + player.transform.position.y)
            {
                isMoving = false;
            }

            // プレイヤーと一緒にブロックを動かす
            Vector3 pos = player.transform.position + offsetPos;
            pos.y = defaultPos.y;
            parentBlock.transform.position = pos;
        }
        else
        {
            parentBlock.GetComponent<BoxCollider2D>().enabled = true;
        }

        // 指定の位置まで運ぶと消える
        if (0.1 < this.transform.position.x - actionPoint.transform.position.x)
        {
            switchClock.Invoke();
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.1f);
            this.GetComponent<BoxCollider2D>().enabled = false;
            this.enabled = false;
            parentBlock.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    public void MoveWithPlayer()
    {
        isMoving = !isMoving;
        offsetPos = transform.position - player.transform.position;
    }

    PlatformerModel model = Simulation.GetModel<PlatformerModel>();
    void OnTriggerEnter2D(Collider2D collider)
    {
        var p = collider.gameObject.GetComponent<Platformer.Mechanics.PlayerController>();
        if (p != null)
        {
            // blockを押してる間、プレイヤーの速度を下げる
            tmpSpeed = model.player.maxSpeed;
            model.player.maxSpeed = limitedSpeed;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        var p = collider.gameObject.GetComponent<Platformer.Mechanics.PlayerController>();
        if (p != null)
        {
            // プレイヤーの速度を戻す
            model.player.maxSpeed = tmpSpeed;
        }
    }
}
