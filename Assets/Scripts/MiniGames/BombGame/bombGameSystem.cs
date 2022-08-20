using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombGameSystem : MonoBehaviour
{
    public int MAX_POSITION = 47;
    public GameObject bombObject;
    public GameObject playerObject;
    private RectTransform bombTransform;
    private RectTransform playerTransform;
    private Bomb bomb;
    private bool tookAction;
    private int bombSpeed = 1;
    private float bombFallSpeed = 90f;
    private float playerSpeed = 50f;

    void OnEnable(){
        tookAction = false;
        bombTransform = bombObject.GetComponent<RectTransform>();
        playerTransform = playerObject.GetComponent<RectTransform>();
        bomb = new Bomb();
        bombTransform.anchoredPosition = new Vector2(-MAX_POSITION, 30);
    }

    void OnDisable(){

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && !tookAction){
            tookAction = true;
        }

        if(!tookAction){
            moveBomb();
        } else {
            dropBomb();
        }
    }

    void moveBomb(){
        Vector2 bombPosition = bombTransform.anchoredPosition;
 
        switch(GameManager.currentGameState){
            case GameManager.gameState.RoundOne:
                if(bombPosition.x >= MAX_POSITION){
                    bombPosition = new Vector2(-MAX_POSITION, 30);
                    bomb = new Bomb();
                }
                break;
            case GameManager.gameState.RoundTwo:
                if(bombPosition.x >= MAX_POSITION || bombPosition.x < -MAX_POSITION){
                    bombSpeed = -bombSpeed;
                }
                break;
            case GameManager.gameState.RoundThree:
                Vector2 playerPosition = playerTransform.anchoredPosition;

                if(bombPosition.x >= MAX_POSITION || bombPosition.x < -MAX_POSITION){
                    bombSpeed = -bombSpeed;
                }
                if(playerPosition.x >= MAX_POSITION || playerPosition.x < -MAX_POSITION){
                    playerSpeed = -playerSpeed;
                }
                playerPosition.x += playerSpeed * Time.deltaTime;
                playerTransform.anchoredPosition = playerPosition;
                break;
            default:
                if(bombPosition.x >= MAX_POSITION || bombPosition.x < -MAX_POSITION){
                    bombSpeed = -bombSpeed;
                }
                break;
        }

        bombPosition.x += bomb.getSpeed() * bombSpeed * Time.deltaTime;

        bombTransform.anchoredPosition = bombPosition;
    }

    void dropBomb(){
        Vector2 position = bombTransform.anchoredPosition;

        if(position.y <= -MAX_POSITION){
            StartCoroutine(endGame()); // IF we want to make a super small bomb animation before destroying the object
            return;
        }

        position.y -= bombFallSpeed * Time.deltaTime;

        bombTransform.anchoredPosition = position;
    }

    IEnumerator endGame(){
        Debug.Log("YOU ENDED THE GAME!");
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        GameManager.playerMove = true;
    }
}

class Bomb{
    private float speed;
    public Bomb(){
        switch(GameManager.currentGameState){
            case GameManager.gameState.RoundOne:
                speed = Random.Range(40f, 80f);
                break;
            case GameManager.gameState.RoundTwo:
                speed = Random.Range(80f, 120f);
                break;
            case GameManager.gameState.RoundThree:
                speed = Random.Range(120f, 200f);
                break;
            default:
                speed = Random.Range(80f, 100f);
                break;
        }
    }

    public float getSpeed(){
        return speed;
    }
}
