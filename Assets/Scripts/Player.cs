using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveAmount;

    // Dash variables
    public float dashSpeed;
    private float activeMoveSpeed;
    private float dashLength = .5f;
    private float dashCooldown = .5f;
    private float dashCounter = 0; 
    private float dashCoolCounter = 0;
    public void Start(){
        rb = GetComponent<Rigidbody2D>();
        activeMoveSpeed = speed;
    }

    public void Update(){

        // no player movement whilst mini game is on screen
        if(GameManager.playerMove){
       
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            // makes sure the player doesn't move faster when moving diagonally!
            moveAmount = moveInput.normalized * activeMoveSpeed;
            if(Input.GetKeyDown(KeyCode.Space) && !GameManager.dodgeDisabled){
                SpeedBurst();
            }

            DecreaseDashCounters();
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
            }
    }

    // contains code related to physics
    private void FixedUpdate() {
        // framerate independent
        rb.MovePosition(rb.position + moveAmount * Time.fixedDeltaTime);
    }
}
