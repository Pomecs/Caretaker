using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBattleManager : MonoBehaviour
{
    public float animationTime;
    public int maxTries;
    public int targetValue;
    public GameObject[] livesObjects;
    public static int currentTarget;
    public static int currentTries;
    private static int score;
    public static bool running;

    void OnEnable()
    {
        running = true;
        currentTarget = targetValue;
        currentTries = maxTries;
        GameManager.startedFinalBattle = true;
    }

    void OnDisable(){
        GameManager.setState(GameManager.gameState.Reset);
        resetGame();
    }

    void resetGame(){
        foreach(GameObject live in livesObjects){
            live.SetActive(true);
        }
        Spawner.removeAllEnemies();
    }
    
    void Update()
    {
        if(currentTries <= 0){ // lose
            livesObjects[0]?.SetActive(false);
            StartCoroutine(endGame());
            return;
        }

        if(currentTries < maxTries){
            livesObjects[currentTries]?.SetActive(false);
        }

        if(currentTarget <= 0){ // win
            GameManager.increaseScore(score);
            StartCoroutine(endGame());
        }
    }

    public static int getTries(){
        return currentTries;
    }

    public static void decreaseTries(){
        currentTries--;
    }

    public static void hitTarget(){
        currentTarget--;
    }

    public static void increaseScore(){
        score += 10;
    }

    IEnumerator endGame(){
        running = false;
        yield return new WaitForSeconds(animationTime);
        gameObject.SetActive(false);
        GameManager.playerMove = true;
    }
}
