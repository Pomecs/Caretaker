using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetector : MonoBehaviour
{
    public AudioClip[] bombSounds;
    private AudioSource audioSource;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col){
        // we can check if player? col.gameObject.tag.Equals("Player")
        if(GameManager.isPlayerAtRightStation){
            GameManager.increaseScore(20);
        }
        audioSource.clip = bombSounds[Random.Range(0, bombSounds.Length - 1)];
        audioSource.PlayOneShot(audioSource.clip);
    }
}
