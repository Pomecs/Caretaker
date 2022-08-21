using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    private float speed;

    void FixedUpdate()
    {
        switch(GameManager.lastGameState){
            case GameManager.gameState.RoundTwo:
                speed = 150;
            break;
            case GameManager.gameState.RoundThree:
                speed = 200;
            break;
            case GameManager.gameState.FinalBattle:
                speed = 250;
            break;
            default:
                speed = 100;
            break;
        }

        transform.Translate(Vector2.left * speed * Time.deltaTime);
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
