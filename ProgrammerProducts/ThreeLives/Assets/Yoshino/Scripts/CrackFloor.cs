using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackFloor : MonoBehaviour
{
    //[SerializeField] private bool button=false;
    //�����܂ł̎���
    [SerializeField] private float crackTime = 0.5f;
    //�ڕW�̊p�x
    [SerializeField] private float targetAngle = -45.0f;
    //private Rigidbody2D rb;
    //�A�j���[�V�������I��������
    private bool isCracked = false;
    //�A�j���[�V��������
    private bool isPlaying = false;
    //�o�ߎ���
    private float elapsedTime = 0.0f;
   
    public void SetMoveButton()
    {
        //�P�x�{�^���������ꂽ�狭���I�ɓ�����
        if (isCracked) return;
        //�o�ߎ��Ԃ����Z�b�g�A�t���O��true�ɂ���
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
            //�C�[�W���O�Ŋp�x���Ԃ���
            float alpha = QuartIn(elapsedTime, crackTime, 0.0f, 1.0f);
            float angle = Mathf.Lerp(0.0f, targetAngle, alpha);
            transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            //�o�ߎ��Ԃ��v�Z
            elapsedTime += Time.deltaTime;
        }
        else
        {
            //�o�ߎ��Ԃ��߂���ΖڕW�̊p�x�ɂ��A�A�j���[�V�������I������
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