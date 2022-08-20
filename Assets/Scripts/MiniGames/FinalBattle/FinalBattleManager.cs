using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBattleManager : MonoBehaviour
{
    public int maxTries;
    public int targetValue;
    public static int currentTarget;
    public static int currentTries;
    private static int score;

    void OnEnable()
    {
        currentTarget = targetValue;
        currentTries = maxTries;
    }

    
    void Update()
    {
        if(currentTries <= 0){ // lose
            gameObject.SetActive(false);
            GameManager.playerMove = true;
        }

        if(currentTarget <= 0){ // win
            GameManager.increaseScore(score);
            gameObject.SetActive(false);
            GameManager.playerMove = true;
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
}
