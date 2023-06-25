using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Clock : MonoBehaviour
{
    public bool isWorking;
    [SerializeField]
    float rotateSpeedScale = 1;
    [SerializeField]
    Transform hourHand, minuteHand, secondHand;

    void Update()
    {
        if (isWorking)
            RotateSecond(Time.deltaTime);
    }
    public void SetIsWorking(bool cond) { isWorking = cond; }
    public void RotateSecond(float second)
    {
        secondHand.eulerAngles -= Vector3.forward * second * 6 * rotateSpeedScale;
    }
    public void SetSecond(float second)
    {
        secondHand.eulerAngles = Vector3.forward * -second * 6 * rotateSpeedScale;
    }
    public void RotateHourAndMinute(float hour)
    {
        hourHand.eulerAngles -= Vector3.forward * hour * 30;
        minuteHand.eulerAngles -= Vector3.forward * hour;
    }
}
