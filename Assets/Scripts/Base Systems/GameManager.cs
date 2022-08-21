using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public float roundTimeLimit;
    public float timeBtwRequests;
    public float lastBattleAnimTime;
    public float timeToLastBattle;
    public int targetRoundScore;
    public InteractableStation holdStation;
    public InteractableStation bombStation;
    public InteractableStation sequenceStation;
    public InteractableStation finalStation;
    public GameObject finalBattleRequest;
    public GameObject[] requests;
    public static GameObject currentRequest;
    public static bool isPlayerAtRightStation;
    public static TextMeshProUGUI timerText;
    public static TextMeshProUGUI scoreText;
    public static TextMeshProUGUI roundText;
    public static TextMeshProUGUI finalBattleText;

    public enum gameState{
        Intro,
        RoundOne,
        RoundTwo,
        RoundThree,
        FinalBattle,
        EndGame,
        Reset
    }

    public static gameState currentGameState;
    private static gameState lastGameState;

    // change to static methods
    public static bool dodgeDisabled = false;
    public static bool playerMove = true;

    private static int score = 0;
    private int finalScore = 0;
    private Timer roundTimer;
    private Timer requestTimer;
    private Timer lastBattleTimer;
    private static bool isRoundFinished;
    public static bool startedFinalBattle;

    void Awake(){
        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        roundText = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
        finalBattleText = GameObject.Find("FinalBattleText").GetComponent<TextMeshProUGUI>();
        finalBattleText.gameObject.SetActive(false);
        currentGameState = gameState.Intro;
        lastGameState = currentGameState;
        score = 0;
        roundTimer = new Timer(roundTimeLimit, false);
        requestTimer = new Timer(timeBtwRequests, true);
        isRoundFinished = false;
        startedFinalBattle = false;
    }

    void Start()
    {
        timerText.text = $"{roundTimer.getTimer():00}";
        updateScoreText();
        updateTimerText();
        updateRoundText();
    }

    void Update()
    {
        if(isRoundFinished){ // changed in resetRound
            currentGameState = lastGameState;
            nextState();
            updateRoundText();
            updateScoreText();
            updateTimerText();
            isRoundFinished = false;
            StartCoroutine(startRound());
        }

        switch(currentGameState){
            case gameState.Intro:

            break;
            case gameState.RoundOne:
                timersUpdate();
            break;
            case gameState.RoundTwo:
                timersUpdate();
            break;
            case gameState.RoundThree:
                timersUpdate();
            break;
            case gameState.FinalBattle: // FIX FINAL BATTLE STATE!!!!
                timersUpdate();
            break;
            case gameState.EndGame:

            break;
            case gameState.Reset:
                resetRound();
            break;
        }
        updateStations();
    }

    IEnumerator startRound(){
        subscribeTimers();
        finalBattleText.text = $"{currentGameState} is about to start!";
        finalBattleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        finalBattleText.gameObject.SetActive(false);
        roundTimer.setTimerActive();
        requestTimer.setTimerActive();
        sendNewRequest();
    }

    private void timersUpdate(){
        roundTimer.update();
        requestTimer.update();
        lastBattleTimer?.update();
    }

    public void updateStations(){
        switch(currentGameState){
            case gameState.Intro:
                holdStation.setDisabled(false);
                sequenceStation.setDisabled(true);
                bombStation.setDisabled(true);
                finalStation.setDisabled(true);
            break;
            case gameState.FinalBattle:
                holdStation.setDisabled(true);
                sequenceStation.setDisabled(true);
                bombStation.setDisabled(true);
                finalStation.setDisabled(false);
            break;
            default:
                holdStation.setDisabled(false);
                sequenceStation.setDisabled(false);
                bombStation.setDisabled(false);
                finalStation.setDisabled(true);
            break;
        }
    }

    public static void nextState(){
        currentGameState++;
    }

    public static void setState(gameState state){
        currentGameState = state;
    }

    public static void increaseScore(int value){
        score += value;
        updateScoreText();
    }

    public static void decreaseScore(){
        score -= 10;
        updateScoreText();
    }

    private static void updateScoreText(){
        scoreText.text = score.ToString();
    }

    public void sendNewRequest(){ // Maybe set isPlayerAtRightStation to false here
        currentRequest?.SetActive(false);

        int rand = UnityEngine.Random.Range(0, requests.Length);
        currentRequest = requests[rand];
        currentRequest.SetActive(true);
    }

    public void requestFinalBattle(){
        currentRequest?.SetActive(false);
        currentRequest = finalBattleRequest;
        currentRequest.SetActive(true);
    }

    public void updateTimerText(){
        if(roundTimer.isTimerRunning()){
            timerText.text = $"{roundTimer.getTimer():00}";
            return;   
        }

        if(lastBattleTimer != null){
            timerText.text = $"{lastBattleTimer.getTimer():00}"; 
        }
    }

    public void updateRoundText(){
        roundText.text = currentGameState.ToString();
    }

    public void resetRound(){
        checkScore();
        currentRequest.SetActive(false);
        currentRequest = null;
        roundTimer = new Timer(roundTimeLimit, false);
        requestTimer = new Timer(timeBtwRequests, true);
        score = 0;
        currentGameState = lastGameState;
        isRoundFinished = true;
    }

    public void checkScore(){
        if(score < targetRoundScore){ // If round is lost, dont save score!
            Debug.Log("YOU LOST THE ROUND!!!!");
        } else {
            Debug.Log("YOU WON THE ROUND!!!!");
            finalScore += score;
        }
    }

    public void subscribeTimers(){
        requestTimer.onTimerEnd += sendNewRequest;
        roundTimer.onTimerStart += updateTimerText;
        roundTimer.onTimerEnd += setupFinalBattle;
    }

    public void checkFinalEncounter(){
        if(startedFinalBattle){
            return;
        }
        
        resetRound();
    }

    void setupFinalBattle(){
        lastBattleTimer = new Timer(timeToLastBattle, false);
        lastBattleTimer.onTimerStart += updateTimerText;
        lastBattleTimer.onTimerEnd += checkFinalEncounter;
        finalBattleText.text = "FINAL BATTLE!";
        finalBattleText.gameObject.SetActive(true);
        StartCoroutine(startFinalBattle());
    }

    public IEnumerator startFinalBattle(){
        lastGameState = currentGameState;
        yield return new WaitForSeconds(lastBattleAnimTime);

        currentGameState = gameState.FinalBattle;
        finalBattleText.gameObject.SetActive(false);
        lastBattleTimer.setTimerActive(); // starting
        requestTimer.setTimerActive(); // turning it off

        requestFinalBattle();
    }

    public static void setRoundFinished(bool value){
        isRoundFinished = value;
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