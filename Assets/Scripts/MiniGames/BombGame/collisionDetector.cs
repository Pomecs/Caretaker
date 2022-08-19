using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetector : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        // we can check if player? col.gameObject.tag.Equals("Player")

        GameManager.increaseScore();      
    }
}
