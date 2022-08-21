using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceGame : MonoBehaviour
{
    public AudioClip[] hitSounds;
    private ProgressBar progressBar;
    private KeyCode previousKey = KeyCode.None;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        progressBar = GetComponentInChildren<ProgressBar>();
    }

    void Update()
    {

        if(previousKey != KeyCode.None){
            if(Input.GetKeyDown(KeyCode.LeftArrow) && previousKey != KeyCode.LeftArrow){
                audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length - 1)];
                audioSource.PlayOneShot(audioSource.clip);
                progressBar.IncreaseValue();
                previousKey = KeyCode.LeftArrow;
            } else if(Input.GetKeyDown(KeyCode.RightArrow) && previousKey != KeyCode.RightArrow){
                audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length - 1)];
                audioSource.PlayOneShot(audioSource.clip);
                progressBar.IncreaseValue();
                previousKey = KeyCode.RightArrow;
            }
        }


        if(previousKey == KeyCode.None && Input.GetKeyDown(KeyCode.LeftArrow)){
            previousKey = KeyCode.LeftArrow;
        } else if (previousKey == KeyCode.None && Input.GetKeyDown(KeyCode.RightArrow)) {
            previousKey = KeyCode.RightArrow;
        }
    }

    public void Disable(){
        gameObject.SetActive(false);
        GameManager.playerMove = true;
    }

    
}
