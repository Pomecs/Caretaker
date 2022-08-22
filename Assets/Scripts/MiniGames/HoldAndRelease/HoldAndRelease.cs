using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldAndRelease : MonoBehaviour
{
    public AudioClip[] winSounds;
    public AudioClip[] loseSounds;
    private AudioSource audioSource;
    private Slider bar;
    public float speed;
    private int maxBar = 100;
    private int currentBar;
    private bool pressed;

    public GameObject gameOb;
    private ObjectiveBox ob;
    
    private void OnEnable(){
         audioSource = GetComponent<AudioSource>();
        bar = GetComponentInChildren<Slider>();
        ob = gameOb.GetComponent<ObjectiveBox>();
        currentBar = maxBar;
        bar.maxValue = maxBar;
        bar.value = 0;
        setUpSpeed();
    }

    void setUpSpeed(){
        switch(GameManager.currentGameState){
            case GameManager.gameState.RoundTwo:
                speed = Random.Range(0.4f, 0.5f);
            break;
            case GameManager.gameState.RoundThree:
                speed = Random.Range(0.4f, 0.4f);
            break;
            case GameManager.gameState.FinalBattle:
                speed = Random.Range(0.4f, 0.5f);
            break;
            default:
                speed = 0.3f;
            break;
        }
    }


   private void Update() {
    
        if(Input.GetKeyDown(KeyCode.DownArrow) && !pressed){
            pressed = true;
        } 

        if(pressed){
            bar.value += speed;
        }

        if(Input.GetKeyUp(KeyCode.DownArrow)){
            pressed = false;

            if(ob.WithinRange(bar.value)){
                if(GameManager.currentGameState != GameManager.gameState.Intro && GameManager.isPlayerAtRightStation){
                    GameManager.increaseScore(10);
                }    
                audioSource.clip = winSounds[Random.Range(0, winSounds.Length - 1)];
                audioSource.PlayOneShot(audioSource.clip);
            } else {
                audioSource.clip = loseSounds[Random.Range(0, loseSounds.Length - 1)];
                audioSource.PlayOneShot(audioSource.clip);
            }

            gameObject.SetActive(false);
            GameManager.playerMove = true;

            if(GameManager.currentGameState == GameManager.gameState.Intro){
                GameManager.setRoundFinished(true);
            }
        }
   }
}
