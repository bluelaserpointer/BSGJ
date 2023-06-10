using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

//mizukiさんのBlockMove.csのコピペ
public class PushObject : MonoBehaviour
{
    GameObject player;
    GameObject childBlock;
    bool isMoving = false;
    Vector3 offsetPos;
    Vector3 defaultPos;
    [SerializeField]
    float limitedSpeed = 1;
    float tmpSpeed;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        defaultPos = transform.position;
        childBlock = transform.GetChild(0).gameObject;
    }
    private void Update()
    {
        if (isMoving)
        {
            // ブロックの当たり判定が邪魔して押し進めないのでcolliderを消す もうちょっといい処理あるかも
            //childBlock.GetComponent<BoxCollider2D>().enabled = false;
            childBlock.GetComponent<CircleCollider2D>().enabled = false;
            childBlock.GetComponent<Rigidbody2D>().gravityScale = 0;
            // ジャンプすると解除
            if (0.1 < offsetPos.y - transform.position.y + player.transform.position.y)
            {
                isMoving = false;
            }

            // プレイヤーと一緒にブロックを動かす
            Vector3 pos = player.transform.position + offsetPos;
            pos.y = defaultPos.y;
            transform.position = pos;
        }
        else
        {
            //childBlock.GetComponent<BoxCollider2D>().enabled = true;
            childBlock.GetComponent<CircleCollider2D>().enabled = true;
            childBlock.GetComponent<Rigidbody2D>().gravityScale = 1;
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