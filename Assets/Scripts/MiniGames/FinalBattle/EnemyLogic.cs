using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLogic : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other){

        if(other.tag == "Player"){
            Destroy(gameObject);
            FinalBattleManager.decreaseTries();
            return;
        }

        if(other.tag == gameObject.tag){
            Destroy(other.gameObject);
            Destroy(gameObject);
            FinalBattleManager.increaseScore();
            FinalBattleManager.hitTarget();
        } else {
            Destroy(other.gameObject);
            Destroy(gameObject);
            FinalBattleManager.decreaseTries();
        }
    }
}
