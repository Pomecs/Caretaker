using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ParticleSystem footstepsParticle;
    public float speed;
    public AudioClip[] footstepSounds;
    public AudioClip[] dashSounds;
    private Rigidbody2D rb;
    private Vector2 moveAmount;
    private Animator anim;

    // Dash variables
    public float dashSpeed;
    private float activeMoveSpeed;
    private float dashLength = .5f;
    private float dashCooldown = .5f;
    private float dashCounter = 0; 
    private float dashCoolCounter = 0;
    private AudioSource audioSource;

    void Awake(){
        audioSource = GetComponent<AudioSource>();
    }
    public void Start(){
        rb = GetComponent<Rigidbody2D>();
        activeMoveSpeed = speed;
        anim = GetComponent<Animator>();
    }

    public void Update(){

        // no player movement whilst mini game is on screen
        if(GameManager.playerMove){
       
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if(moveInput != Vector2.zero){
                anim.SetBool("isRunning", true);
                audioSource.clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
                if(!audioSource.isPlaying){
                    audioSource.PlayOneShot(audioSource.clip);
                }
            } else {
                anim.SetBool("isRunning", false);
            }
            // makes sure the player doesn't move faster when moving diagonally!
            moveAmount = moveInput.normalized * activeMoveSpeed;
            if(Input.GetKeyDown(KeyCode.Space) && !GameManager.dodgeDisabled){
                SpeedBurst();
            }

            DecreaseDashCounters();
        } else {
            moveAmount = Vector2.zero;
        }
    }

    private void DecreaseDashCounters(){
        if(dashCounter > 0) {
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0){
                activeMoveSpeed = speed;
                dashCoolCounter = dashCooldown;
            }
        }

        if(dashCoolCounter > 0){
            dashCoolCounter -= Time.deltaTime;
        }
    }

    private void SpeedBurst(){
       if(dashCoolCounter <= 0 && dashCounter <= 0){
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            audioSource.clip = dashSounds[Random.Range(0, dashSounds.Length - 1)];
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    // contains code related to physics
    private void FixedUpdate() {
        // framerate independent
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
    }
}
