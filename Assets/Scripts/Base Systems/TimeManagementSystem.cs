using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManagementSystem : MonoBehaviour
{
    public Action onTimerStart;  
    public Action onTimerEnd;
    private float startTimer;
    private float timer;
    private bool loop;
    private bool timerRunning;
    private bool timerFinished;

    public TimeManagementSystem(float timer, bool loop){
        startTimer = timer;
        this.timer = startTimer;
        this.loop = loop;
        timerRunning = false;
        timerFinished = false;
    }

    void Update()
    {
        if(timerRunning){
            onTimerStart?.Invoke(); // Checks if onTimerStart is not null and then invoke it.
            Debug.Log("Firing an event on timer");
            timer -= Time.deltaTime;

            if(timer <= 0){
                if(!loop){
                    setTimerActive();
                    timerFinished = true;
                }

                onTimerEnd?.Invoke();
                timer = startTimer;
            }
        }
    }

    public float getTimer(){
        return timer;
    }

    public bool isTimerFinished(){
        return timerFinished;
    }

    public void setTimer(float value){
        timer = value;
    }

    public void setTimerActive(){
        timerRunning = !timerRunning;
    }

    public bool isTimerRunning(){
        return timerRunning;
    }
}
