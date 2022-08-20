using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public float roundTimeLimit;
    public float timeBtwRequests;
    public int targetRoundScore;
    public static TextMeshProUGUI timerText;
    public static TextMeshProUGUI scoreText;
    public static TextMeshProUGUI roundText;

    public enum gameState{
        Intro,
        RoundOne,
        RoundTwo,
        RoundThree,
        FinalBattle,
        EndGame
    }

    public static gameState currentGameState;

    // change to static methods
    public static bool dodgeDisabled = false;
    public static bool playerMove = true;

    private static int score = 0;
    private int finalScore = 0;
    private Timer roundTimer;
    private Timer requestTimer;
    void Awake(){
        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        roundText = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
        currentGameState = gameState.Intro;
        score = 0;
        roundTimer = new Timer(roundTimeLimit, false);
        requestTimer = new Timer(timeBtwRequests, true);
    }

    void Start()
    {
        updateScoreText();
        updateTimerText();
        updateRoundText();
        subscribeTimers();
    }

    void Update()
    {
        roundTimer.update();
        requestTimer.update();

        if (Input.GetKeyDown("m")){
            roundTimer.setTimerActive();
            requestTimer.setTimerActive();
            sendNewRequest();
        }
    }

    public static void nextState(){
        currentGameState++;
    }

    public static void increaseScore(int value){
        score += value;
        updateScoreText();
    }

    public static void decreaseScore(){
        score -= 10;
        updateScoreText();
    }

    public static int getScore(){
        return score;
    }

    private static void updateScoreText(){
        scoreText.text = score.ToString();
    }

    public void sendNewRequest(){
        Debug.Log("SEnding a new Request");
    }

    public void updateTimerText(){
        timerText.text = $"{roundTimer.getTimer():00.00}";
    }

    public void updateRoundText(){
        roundText.text = currentGameState.ToString();
    }

    private void resetRound(){//round ends, check for score and show if game is won
        checkScore();
        finalScore += score;
        roundTimer = new Timer(roundTimeLimit, false);
        requestTimer = new Timer(timeBtwRequests, true);
        score = 0;
        nextState();
        subscribeTimers();
        updateRoundText();
        updateScoreText();
        updateTimerText();
    }

    private void checkScore(){
        if(score < targetRoundScore){
            Debug.Log("YOU LOST THE ROUND!!!!");
        } else {
            Debug.Log("YOU WON THE ROUND!!!!");
        }
    }

    private void subscribeTimers(){
        requestTimer.onTimerEnd += sendNewRequest;
        roundTimer.onTimerStart += updateTimerText;
        roundTimer.onTimerEnd += resetRound;
    }
}

class Timer {

    public Action onTimerStart;  
    public Action onTimerEnd;
    private float startTimer;
    private float timer;
    private bool loop;
    private bool timerRunning;
    private bool timerFinished;

    public Timer(float timer, bool loop){
        startTimer = timer;
        this.timer = startTimer;
        this.loop = loop;
        timerRunning = false;
        timerFinished = false;
    }

    public void update()
    {
        if(timerRunning){
            onTimerStart?.Invoke(); // Checks if onTimerStart is not null and then invoke it.
            timer -= Time.deltaTime;

            if(timer <= 0){
                if(!loop){
                    setTimerActive();
                    timerFinished = true;
                }

                timer = startTimer;
                onTimerEnd?.Invoke();
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