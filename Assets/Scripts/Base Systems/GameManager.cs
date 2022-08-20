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
    public int targetRoundScore;
    public InteractableStation holdStation;
    public InteractableStation bombStation;
    public InteractableStation sequenceStation;
    public InteractableStation finalStation;
    public GameObject finalBattleRequest;
    public GameObject[] requests;
    private GameObject[] currentRequests;
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
        EndGame
    }

    public static gameState currentGameState;
    private gameState lastGameState;

    // change to static methods
    public static bool dodgeDisabled = false;
    public static bool playerMove = true;

    private static int score = 0;
    private int finalScore = 0;
    private Timer roundTimer;
    private Timer requestTimer;
    private Timer lastBattleTimer;
    private GameObject requestSpawner;
    private static bool isRoundFinished;

    void Awake(){
        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        roundText = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
        finalBattleText = GameObject.Find("FinalBattleText").GetComponent<TextMeshProUGUI>();
        requestSpawner = GameObject.Find("RequestSpawner");
        finalBattleText.gameObject.SetActive(false);
        currentGameState = gameState.Intro;
        lastGameState = currentGameState;
        score = 0;
        roundTimer = new Timer(roundTimeLimit, false);
        requestTimer = new Timer(timeBtwRequests, true);
        lastBattleTimer = new Timer(timeBtwRequests, false);
        isRoundFinished = false;
    }

    void Start()
    {
        timerText.text = $"{roundTimer.getTimer():00.00}";
        updateScoreText();
        updateTimerText();
        updateRoundText();
        subscribeTimers();
    }

    void Update()
    {
        if(isRoundFinished){
            currentGameState = lastGameState;
            nextState();
            subscribeTimers();
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
            case gameState.FinalBattle:
                timersUpdate();
            break;
            case gameState.EndGame:

            break;
        }

        updateStations();
    }

    IEnumerator startRound(){
        finalBattleText.text = $"{currentGameState} is about to start!";
        finalBattleText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        finalBattleText.gameObject.SetActive(false);
        roundTimer.setTimerActive();
        requestTimer.setTimerActive();
        sendNewRequest();
    }

    private void checkActiveStation(){

    }

    private void timersUpdate(){
        roundTimer.update();
        requestTimer.update();
        lastBattleTimer.update();
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

    public void sendNewRequest(){ // ADD REQUESTS TO QUEUE. ADD CURRENTGAME, CHECK IF BOTH ARE THE SAME.
        destroyRequests();
        int rand = UnityEngine.Random.Range(0, 100);
        if(rand > 80){
            sendDoubleRequest();
        } else {
            sendSingleRequest();
        }
    }

    private void destroyRequests(){
        foreach(GameObject request in requests){
            DestroyImmediate(request, true);
        }
    }

    private void sendSingleRequest(){ // NEED TO DESTROY OBJECT AFTER 5s, OR STORE IT AND THEN DESTROY IT
        Debug.Log("SEnding a new Request");
        int rand = UnityEngine.Random.Range(0, requests.Length);
        currentRequests.SetValue(PrefabUtility.InstantiatePrefab(requests[rand] as GameObject), 0);
    }

    private void sendDoubleRequest(){ // NEED TO STORE GAMES PLAYED AND RESET THEM AFTER??? SHIT NEED TO THIK...
        Debug.Log("Sending a DOUBLE Request");
        Vector2 pos1 = requestSpawner.transform.position;
        Vector2 pos2 = requestSpawner.transform.position;
        pos1.x -= 2f;
        pos2.x += 2f;
        int rand1 = UnityEngine.Random.Range(0, requests.Length);
        int rand2 = UnityEngine.Random.Range(0, requests.Length);
        currentRequests.SetValue(Instantiate(requests[rand1], pos1, Quaternion.identity), 0);
        currentRequests.SetValue(Instantiate(requests[rand2], pos2, Quaternion.identity), 1);
    }

    public void requestFinalBattle(){
        Debug.Log("RUN TO THE LAST BATTLE!!");
        currentRequests.SetValue(Instantiate(finalBattleRequest, requestSpawner.transform.position, Quaternion.identity), 0);
    }

    public void updateTimerText(){
        if(roundTimer.isTimerRunning()){
            timerText.text = $"{roundTimer.getTimer():00.00}";   
        }
        if(lastBattleTimer.isTimerRunning()){
            timerText.text = $"{lastBattleTimer.getTimer():00.00}";
        }
        
    }

    public void updateRoundText(){
        roundText.text = currentGameState.ToString();
    }

    public void resetRound(){//round ends, check for score and show if game is won
        checkScore();
        currentRequests = new GameObject[2];
        finalScore += score;
        roundTimer = new Timer(roundTimeLimit, false);
        requestTimer = new Timer(timeBtwRequests, true);
        score = 0;
        isRoundFinished = true;
    }

    public void checkScore(){
        if(score < targetRoundScore){
            Debug.Log("YOU LOST THE ROUND!!!!");
        } else {
            Debug.Log("YOU WON THE ROUND!!!!");
        }
    }

    public void subscribeTimers(){
        requestTimer.onTimerEnd += sendNewRequest;
        roundTimer.onTimerStart += updateTimerText;
        roundTimer.onTimerEnd += setupFinalBattle;
    }

    public void checkFinalEncounter(){ // NOT WORKING!!!!!!!! ITS ASYNCH!!!
        // disable final battle station
        Debug.Log("SHOULDNT BE RESETTING!!!!");
        resetRound();
    }

    public void setupFinalBattle(){
        lastBattleTimer.onTimerStart += updateTimerText;
        lastBattleTimer.onTimerEnd += checkFinalEncounter;
        finalBattleText.text = "FINAL BATTLE!";
        finalBattleText.gameObject.SetActive(true);
        StartCoroutine(startFinalBattle());
    }

    public IEnumerator startFinalBattle(){
        lastGameState = currentGameState;
        yield return new WaitForSeconds(2);
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