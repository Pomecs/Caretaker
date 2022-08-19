using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SecondTimerTest : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    // Subscribes to Action in TimeManagementSystem ( Observer Pattern )
    private void OnEnable(){
        TimeManagementSystem.timer = 10f;
        TimeManagementSystem.onTimerStart += updateTimer;
        TimeManagementSystem.onTimerEnd += showResult;
    }

    // Unsubscribes to Action
    private void OnDisable(){
        TimeManagementSystem.onTimerStart -= updateTimer;
        TimeManagementSystem.onTimerEnd -= showResult;
    }

    private void updateTimer(){
        timerText.text = $"{TimeManagementSystem.timer:00.00}"; // String literal with format type ie: 90 seconds and not 90.02341...
    }

    private void showResult(){
        timerText.text = "YOU Suck!!!";
    }
}
