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
    public GameObject[] requests;
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

    // change to static methods
    public static bool dodgeDisabled = false;
    public static bool playerMove = true;

    private static int score = 0;
    private int finalScore = 0;
    private Timer roundTimer;
    private Timer requestTimer;
    private Timer lastBattleTimer;
    private static bool startedFinalBattle;
    private static bool endedFinalBattle;
    private GameObject requestSpawner;
    void Awake(){
        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        roundText = GameObject.Find("RoundText").GetComponent<TextMeshProUGUI>();
        finalBattleText = GameObject.Find("FinalBattleText").GetComponent<TextMeshProUGUI>();
        requestSpawner = GameObject.Find("RequestSpawner");
        finalBattleText.gameObject.SetActive(false);
        currentGameState = gameState.Intro;
        score = 0;
        roundTimer = new Timer(roundTimeLimit, false);
        requestTimer = new Timer(timeBtwRequests, true);
        lastBattleTimer = new Timer(timeBtwRequests, false);
        startedFinalBattle = false;
        endedFinalBattle = false;
    }

    void Start()
    {
        //timerText.text = $"{roundTimer.getTimer():00.00}";
        updateScoreText();
        updateTimerText();
        updateRoundText();
        subscribeTimers();
    }

    void Update()
    {
        roundTimer.update();
        requestTimer.update();
        lastBattleTimer.update();

        if (Input.GetKeyDown("m")){
            roundTimer.setTimerActive();
            requestTimer.setTimerActive();
            sendNewRequest();
        }

        if(endedFinalBattle){
            //resetRound();
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

    private static void updateScoreText(){
        scoreText.text = score.ToString();
    }

    public void sendNewRequest(){ // ADD REQUESTS TO QUEUE. ADD CURRENTGAME, CHECK IF BOTH ARE THE SAME.
        int rand = UnityEngine.Random.Range(0, 100);
        if(rand > 80){
            sendDoubleRequest();
        } else {
            sendSingleRequest();
        }
    }

    private void sendSingleRequest(){ // NEED TO DESTROY OBJECT AFTER 5s, OR STORE IT AND THEN DESTROY IT
        Debug.Log("SEnding a new Request");
        int rand = UnityEngine.Random.Range(0, requests.Length);
        Instantiate(requests[rand], requestSpawner.transform.position, Quaternion.identity);
    }

    private void sendDoubleRequest(){ // NEED TO STORE GAMES PLAYED AND RESET THEM AFTER??? SHIT NEED TO THIK...
        Debug.Log("Sending a DOUBLE Request");
        Vector2 pos1 = requestSpawner.transform.position;
        Vector2 pos2 = requestSpawner.transform.position;
        pos1.x -= 0.5f;
        pos2.x += 0.5f;
        int rand1 = UnityEngine.Random.Range(0, requests.Length);
        int rand2 = UnityEngine.Random.Range(0, requests.Length);
        Instantiate(requests[rand1], pos1, Quaternion.identity);
        Instantiate(requests[rand2], pos2, Quaternion.identity);
    }

    public void requestFinalBattle(){
        Debug.Log("RUN TO THE LAST BATTLE!!");
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
        finalScore += score;
        roundTimer = new Timer(roundTimeLimit, false);
        requestTimer = new Timer(timeBtwRequests, true);
        score = 0;
        //setFinalBattleEnd();
        //setFinalBattleStart();
        nextState();
        subscribeTimers();
        updateRoundText();
        updateScoreText();
        updateTimerText();
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
        if(!getFinalBattleStart()){
            // disable final battle station
            Debug.Log("SHOULDNT BE RESETTING!!!!");
            resetRound();
            return;
        }

    }

    public void setupFinalBattle(){
        lastBattleTimer.onTimerStart += updateTimerText;
        lastBattleTimer.onTimerEnd += checkFinalEncounter;
        finalBattleText.text = "FINAL BATTLE!";
        finalBattleText.gameObject.SetActive(true);
        StartCoroutine(startFinalBattle());
    }

    public IEnumerator startFinalBattle(){
        yield return new WaitForSeconds(2);
        finalBattleText.gameObject.SetActive(false);
        lastBattleTimer.setTimerActive(); // starting
        requestTimer.setTimerActive(); // turning it off
        requestFinalBattle();
    }

    private bool getFinalBattleStart(){
        return startedFinalBattle;
    }

    public static void setFinalBattleStart(){
        startedFinalBattle = !startedFinalBattle;
    }

    public static void setFinalBattleEnd(){
        endedFinalBattle= !endedFinalBattle;
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