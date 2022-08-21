using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bombGameSystem : MonoBehaviour
{
    public float animationTime;
    public int MAX_POSITION;
    public GameObject bombObject;
    public GameObject playerObject;
    public GameObject enemyObject;
    public GameObject deadEnemy;
    private RectTransform bombTransform;
    private RectTransform playerTransform;
    private RectTransform enemyTransform;
    private Bomb bomb;
    private bool tookAction;
    private bool gameFinished;
    private int bombSpeed = 1;
    private float bombFallSpeed = 90f;
    private float playerSpeed = 50f;

    void OnEnable(){
        deadEnemy.SetActive(false);
        enemyObject.SetActive(true);
        tookAction = false;
        bombTransform = bombObject.GetComponent<RectTransform>();
        playerTransform = playerObject.GetComponent<RectTransform>();
        enemyTransform = enemyObject.GetComponent<RectTransform>();
        bomb = new Bomb();
        bombTransform.anchoredPosition = new Vector2(-MAX_POSITION, 30);
        playerTransform.anchoredPosition = new Vector2(-MAX_POSITION, 30);
        enemyTransform.anchoredPosition = new Vector2(0, -22);
        gameFinished = false;
    }

    void OnDisable(){

    }

    // Update is called once per frame
    void Update()
    {
        if(gameFinished) return;

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
        Vector2 playerPosition = playerTransform.anchoredPosition;
 
        switch(GameManager.currentGameState){
            case GameManager.gameState.RoundOne:
                if(bombPosition.x >= MAX_POSITION){
                    bombPosition = new Vector2(-MAX_POSITION, 30);
                    playerPosition = new Vector2(-MAX_POSITION, 30);
                    bomb = new Bomb();
                }
                break;
            case GameManager.gameState.RoundTwo:
                if(bombPosition.x >= MAX_POSITION || bombPosition.x < -MAX_POSITION){
                    bombSpeed = -bombSpeed;
                }
                break;
            case GameManager.gameState.RoundThree:
                Vector2 enemyPosition = enemyTransform.anchoredPosition;

                if(bombPosition.x >= MAX_POSITION || bombPosition.x < -MAX_POSITION){
                    bombSpeed = -bombSpeed;
                }
                if(enemyPosition.x >= MAX_POSITION || enemyPosition.x < -MAX_POSITION){
                    playerSpeed = -playerSpeed;
                }
                enemyPosition.x += playerSpeed * Time.deltaTime;
                enemyTransform.anchoredPosition = enemyPosition;
                break;
            default:
                if(bombPosition.x >= MAX_POSITION || bombPosition.x < -MAX_POSITION){
                    bombSpeed = -bombSpeed;
                }
                break;
        }

        playerPosition.x += bomb.getSpeed() * bombSpeed * Time.deltaTime;
        playerTransform.anchoredPosition = playerPosition;
        bombPosition.x += bomb.getSpeed() * bombSpeed * Time.deltaTime;
        bombTransform.anchoredPosition = bombPosition;
    }

    void dropBomb(){
        Vector2 position = bombTransform.anchoredPosition;

        if(position.y <= -MAX_POSITION){
            deadEnemy.transform.position = enemyTransform.position;
            deadEnemy.SetActive(true);
            enemyObject.SetActive(false);

            StartCoroutine(endGame()); // IF we want to make a super small bomb animation before destroying the object
            gameFinished = true;
            return;
        }

        position.y -= bombFallSpeed * Time.deltaTime;

        bombTransform.anchoredPosition = position;
    }

    IEnumerator endGame(){

        yield return new WaitForSeconds(animationTime);
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
