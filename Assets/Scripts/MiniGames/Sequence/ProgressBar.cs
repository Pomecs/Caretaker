using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    private Slider bar;
    private float speed;
    private int maxBar = 100;
    private int currentBar;
    
    private void OnEnable(){
        bar = GetComponent<Slider>();
        currentBar = maxBar;
        bar.maxValue = maxBar;
        bar.value = 50;
        setSpeed();
    }

    void Update()
    {
        if(bar.value == 0){
            FindObjectOfType<SequenceGame>().Disable();
        } 

        if(bar.value == maxBar){
            if(GameManager.isPlayerAtRightStation){
                GameManager.increaseScore(10);
            }

            FindObjectOfType<SequenceGame>().Disable();
        }

        bar.value -= Time.deltaTime * speed;
    }

    void setSpeed(){
        switch(GameManager.currentGameState){
            case GameManager.gameState.RoundTwo:
                speed = Random.Range(6, 9);
            break;
            case GameManager.gameState.RoundThree:
                speed = Random.Range(10, 14);
            break;
            default:
                speed = Random.Range(4, 6);
            break;
        }
    }

    public void IncreaseValue(){
        switch(GameManager.currentGameState){
            case GameManager.gameState.RoundTwo:
                bar.value += Random.Range(3, 5);
            break;
            case GameManager.gameState.RoundThree:
                bar.value += Random.Range(2, 4);
            break;
            default:
                bar.value += Random.Range(4, 6);
            break;
        }
    }  
}
