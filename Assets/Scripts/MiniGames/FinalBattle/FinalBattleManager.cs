using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBattleManager : MonoBehaviour
{
    public float animationTime;
    private int maxTries;
    private int targetValue;
    public GameObject[] livesObjects;
    public static int currentTarget;
    public static int currentTries;
    private static int score;
    public static bool running;

    void OnEnable()
    {
        running = true;
        setTries();
        GameManager.startedFinalBattle = true;
        setUpGame();
    }

    void OnDisable(){
        GameManager.setState(GameManager.gameState.Reset);
        resetGame();
    }

    void setUpGame(){
        for(int i = 0; i < maxTries; i++){
            livesObjects[i].SetActive(true);
        }
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

        if(currentTarget <= 0 && running){ // win
            running = false;
            StartCoroutine(endGame());
        }
    }

    void setTries(){
        switch(GameManager.currentGameState){
            case GameManager.gameState.RoundOne:
                maxTries = 5;
                targetValue = 15;
            break;
            case GameManager.gameState.RoundTwo:
                maxTries = 4;
                targetValue = 20;
            break;
            case GameManager.gameState.RoundThree:
                maxTries = 3;
                targetValue = 25;
            break;
            default:
                maxTries = 5;
                targetValue = 15;
            break;
        }
        currentTarget = targetValue;
        currentTries = maxTries;
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
        GameManager.increaseScore(score);
        yield return new WaitForSeconds(animationTime);
        gameObject.SetActive(false);
        GameManager.playerMove = true;
    }
}
