using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
    Timer

    Controls the behavior of the timer
*/
public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeLeftText;
    
    public bool timerOn = false;

    
    private float timeElapsed;
    public float timerDuration; // timer duration in seconds
   

    public static Timer Instance { get; private set; }

    public void Awake(){
        Instance = this;
       
    }

    /*
        Starts a 60 second timer
    */
    public void StartTimer()
    {
        timeElapsed = 0;
        timerDuration = 60.0f;
        timerOn = true;
        
    }

   
    /*
        Displays a decrementing timer unitil 60 seconds is up. Then, calls TakeTurn() to make another
        player the artist. 
    */
    void Update()
    {
        if(timerOn){
            timeElapsed += Time.deltaTime; // add the time since the last frame to the timer
            UpdateTimer();
            if (timeElapsed >= timerDuration)
            {
                Debug.Log("Times up!");
                timerOn = false;
                if(PlayerList.Instance.getIsArtist() == true)
                    GameBehavior.Instance.TakeTurn();
            // the timer has expired, do something
            }

        }
        
    }

    /*
        Helper method to display timer
    */
    void UpdateTimer(){
        float timeRemaining = timerDuration - timeElapsed;
        string timeText = string.Format("{0:0.00}", timeRemaining);
        timeLeftText.text = "Time: " + timeText;
    }
    
    public void SetTimerDuration(float duration)
    {
        timerDuration = duration;
    }
    
}

