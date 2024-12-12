using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Timer
{
    UnityEvent timerEnd;
    UnityEvent<float> timerUpdate;
    MonoBehaviour mono;
    bool running;

    public Timer(MonoBehaviour mono, UnityEvent timerEnd, UnityEvent<float> timerUpdate)
    {
        this.mono = mono;
        this.timerEnd = timerEnd;
        this.timerUpdate = timerUpdate;
    }
    public void startTimer(float seconds, float updateIntervallSeconds)
    {
        if (seconds < updateIntervallSeconds)
        {
            return;
        }
        running = true;
        mono.StartCoroutine(CountdownWithUpdate(seconds, updateIntervallSeconds));
    }

    IEnumerator CountdownWithUpdate(float seconds, float updateIntervallSeconds)
    {
        float endTime = Time.time + seconds;

        while (running && Time.time < endTime)
        {
            timerUpdate.Invoke(1 - (endTime - Time.time) / seconds);
            yield return new WaitForSeconds(updateIntervallSeconds);
        }
        if (running)
        {
            timerUpdate.Invoke(1);
            timerEnd.Invoke();
        }
    }

    public void stopTimer()
    {
        running = false;
        timerUpdate.Invoke(-1);
    }
}
