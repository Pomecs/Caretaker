using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BombGameTrigger : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public GameObject gameToPlay;
    private bool isPlayingGame;

    void Start(){
        isPlayingGame = false;
    }

    // Subscribes to Action in TimeManagementSystem ( Observer Pattern )
    private void OnEnable(){
        TimeManagementSystem.timer = 60f;
        TimeManagementSystem.onTimerStart += updateTimer;
        TimeManagementSystem.onTimerEnd += showResult;
    }

    // Unsubscribes to Action
    private void OnDisable(){
        TimeManagementSystem.onTimerStart -= updateTimer;
        TimeManagementSystem.onTimerEnd -= showResult;
    }

    private void updateTimer(){
        if(!isPlayingGame){
            gameToPlay.SetActive(true);
            isPlayingGame = true;
        }

        timerText.text = $"{TimeManagementSystem.timer:00.00}"; // String literal with format type ie: 90 seconds and not 90.02341...
    }

    private void showResult(){
        timerText.text = "YOU SUCK!!!";
        gameToPlay.SetActive(false);
        isPlayingGame = false;
    }
}
