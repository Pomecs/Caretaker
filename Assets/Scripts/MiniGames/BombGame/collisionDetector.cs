using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetector : MonoBehaviour
{

    public GameObject deadSprite;
    

    private void OnEnable() {
        deadSprite.SetActive(false);  

    }
    void OnTriggerEnter2D(Collider2D col){
        // we can check if player? col.gameObject.tag.Equals("Player")
        if(GameManager.isPlayerAtRightStation){
            deadSprite.SetActive(true);
            gameObject.SetActive(false);
            GameManager.increaseScore(20);
        }    
    }
}
