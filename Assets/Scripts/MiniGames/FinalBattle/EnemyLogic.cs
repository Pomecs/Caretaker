using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public float speed;
    void Update()
    {
        switch(GameManager.currentGameState){
            case GameManager.gameState.RoundOne:
                transform.Translate(Vector2.left * speed * Time.deltaTime);
            break;
            case GameManager.gameState.RoundTwo:
                transform.Translate(Vector2.left * speed * 1.5f * Time.deltaTime);
            break;
            case GameManager.gameState.RoundThree:
                transform.Translate(Vector2.left * speed * 2 * Time.deltaTime);
            break;
            default:
                transform.Translate(Vector2.left * speed * 1.5f * Time.deltaTime);
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){

        if(other.tag == "Player"){
            Spawner.removeEnemy(gameObject);
            FinalBattleManager.decreaseTries();
            return;
        }

        if(other.tag == gameObject.tag){ // other is player projectile
            Spawner.removeEnemy(gameObject);
            Destroy(other.gameObject);
            FinalBattleManager.increaseScore();
            FinalBattleManager.hitTarget();
        } else {
            Spawner.removeEnemy(gameObject);
            Destroy(other.gameObject);
            FinalBattleManager.decreaseTries();
        }
    }
}
