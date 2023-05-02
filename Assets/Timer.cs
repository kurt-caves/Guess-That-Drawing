using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeLeftText;
    
    public bool timerOn = false;

    
    private float timeElapsed;
    public float timerDuration; // timer duration in seconds
    // Start is called before the first frame update
    
    public static Timer Instance { get; private set; }

    public void Awake(){
        Instance = this;
       
    }


    public void StartTimer()
    {
        timeElapsed = 0;
        timerDuration = 60.0f;
        timerOn = true;
        
    }

    // Update is called once per frame
    
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
    void OnGUI()
    {
        float timeRemaining = timerDuration - timeElapsed;
        string timeText = string.Format("{0:0.00}", timeRemaining);
        GUI.Label(new Rect(10, 10, 100, 20), "Time: " + timeText);
    }
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

