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
        score = 0;
        running = true;
        setTarget();
        GameManager.startedFinalBattle = true;
        maxTries = 5;
        currentTries = maxTries;
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

        if(currentTarget <= 0 && running){ // win
            running = false;
            GameManager.increaseScore(score);
            StartCoroutine(endGame());
        }
    }

    void setTarget(){
        switch(GameManager.lastGameState){
            case GameManager.gameState.RoundOne:
                targetValue = 15;
            break;
            case GameManager.gameState.RoundTwo:
                targetValue = 20;
            break;
            case GameManager.gameState.RoundThree:
                targetValue = 25;
            break;
            default:
                targetValue = 20; // also works with final battle stage
            break;
        }
        currentTarget = targetValue;
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
        yield return new WaitForSeconds(animationTime);
        gameObject.SetActive(false);
        GameManager.playerMove = true;
    }
}
