using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Timer timer;
    private float timeElapsed;
    public float timerDuration = 60.0f; // timer duration in seconds
    // Start is called before the first frame update
    void Start()
    {
        timer.timerDuration = 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime; // add the time since the last frame to the timer
        if (timeElapsed >= timerDuration)
        {
            Debug.Log("Time's up!");
            // the timer has expired, do something
        }
    }

    void OnGUI()
    {
        float timeRemaining = timerDuration - timeElapsed;
        string timeText = string.Format("{0:0.00}", timeRemaining);
        GUI.Label(new Rect(10, 10, 100, 20), "Time: " + timeText);
    }

    public void SetTimerDuration(float duration)
    {
        timerDuration = duration;
    }
}

