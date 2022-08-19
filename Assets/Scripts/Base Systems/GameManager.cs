using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static TextMeshProUGUI timerText;
    public static TextMeshProUGUI scoreText;

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
    void Awake(){
        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        currentGameState = gameState.RoundTwo;
        score = 0;
    }
    void Start()
    {
        updateScoreText();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

