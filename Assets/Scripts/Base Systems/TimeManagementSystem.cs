using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManagementSystem : MonoBehaviour
{
    public static Action onTimerStart;  
    public static Action onTimerEnd;
    public static float timer;
    private bool isTimerRunning;

    void Start()
    {
        isTimerRunning = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("a") && !isTimerRunning){ // Erase this line, to trigger setTimerActive() outside this class
            setTimerActive();
        }

        if(isTimerRunning){
            onTimerStart?.Invoke(); // Checks if onTimerStart is not null and then invoke it.
            timer -= Time.deltaTime;

            if(timer <= 0){
                onTimerEnd?.Invoke();
                setTimerActive();
                timer = 0;
            }
        }
    }

    void setTimerActive(){
        isTimerRunning = !isTimerRunning;
    }
}
