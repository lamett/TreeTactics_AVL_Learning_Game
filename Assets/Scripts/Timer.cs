using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public UnityEvent timerEnd;
    public UnityEvent<float> timerUpdate;
    public void startTimer(float seconds, float updateIntervallSeconds)
    {
        if (seconds < updateIntervallSeconds)
        {
            return;
        }
        StartCoroutine(CountdownWithUpdate(seconds, updateIntervallSeconds));
    }

    IEnumerator CountdownWithUpdate(float seconds, float updateIntervallSeconds)
    {
        var start = Time.time;
        float endTime = Time.time + seconds;

        while (Time.time < endTime)
        {
            timerUpdate.Invoke(Time.time / endTime);
            yield return new WaitForSeconds(updateIntervallSeconds);
        }
        timerUpdate.Invoke(1);
        timerEnd.Invoke();
    }
}
