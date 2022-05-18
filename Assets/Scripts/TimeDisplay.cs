using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeDisplay : MonoBehaviour
{
    public float currentTime = 0;
    public bool timerIsRunning = false;
    public Text timeText;
    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            currentTime += Time.deltaTime/1.5f;
            DisplayTime(currentTime);
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float hours = Mathf.FloorToInt(timeToDisplay / 3600);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = Mathf.FloorToInt((timeToDisplay - (int) timeToDisplay) * 100);
        timeText.text = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", hours, minutes, seconds, milliseconds);
    }
}