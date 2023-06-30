using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    //�I����Ԃ̉摜
    [SerializeField] private Sprite On;
    //�I�t��Ԃ̉摜
    [SerializeField] private Sprite Off;
    //�v�b�V����Ԃ̉摜
    [SerializeField] private Sprite Pushing;
    //�v�b�V���摜��\�����鎞��
    [SerializeField] private�@float PushingTime = 0.5f;
    //�{�^���̏��
    [SerializeField] private bool button;
    //��x����������悤�ɂ���
    [SerializeField] private bool keepPush;
    //�X�v���C�g�����_�[
    private SpriteRenderer sr;
    //�o�ߎ���
    private float elapsedTime = 0.0f;
    //keepPush��true�̎��ɁA�{�^���������ꂽ��
    private bool isPushed = false;
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        if (elapsedTime < PushingTime)
        {
            //�v�b�V�����̉摜�ɐ؂�ւ�
            sr.sprite = Pushing;
            //�o�ߎ��Ԃ��v�Z
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

    }
    public void Flashing()
    {
        //��x�����������{�^��
        if (keepPush)
        {
            if (isPushed) return;
            isPushed = true;
        }
       ;//�o�ߎ��Ԃ����Z�b�g
        elapsedTime = 0.0f;
        //�g�O�����̃{�^��
        button = button ? button = false : button = true;
    }
}
