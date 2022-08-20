using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStation : MonoBehaviour
{

    private bool inRange = false;
    public int cooldown;
    public GameObject gameToPlay;
    public GameObject requestObject;
    private bool disabled = false;
    private bool isActive = false;
    private SpriteRenderer spriteRenderer;
    
    public void Start() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = Color.yellow;
    }

    public void Update() {

        if(Input.GetKeyDown(KeyCode.Space) && inRange && !disabled){
            isActive = true;
            GameManager.dodgeDisabled = true;
            GameManager.playerMove = false;
            gameToPlay?.SetActive(true);
            // afterwards, trigger cooldown
            TriggerCooldown();
        }
        
    }

    public void setDisabled(bool value){
        disabled = value;
        if(value){
            spriteRenderer.color = Color.red;
        } else {
            spriteRenderer.color = Color.white;
        }
        
    }

    public void TriggerCooldown(){
        setDisabled(true);
        StartCoroutine(StopCooldown(cooldown));
    }

    private IEnumerator StopCooldown(int secs){
        yield return new WaitForSeconds(secs);
        setDisabled(false);
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
      
        // this check isn't necessary for now, but here just in case we add other moving parts later (like powerups or annoying creatures)
        if(other.gameObject.CompareTag("Player")){
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            inRange = false;
            GameManager.dodgeDisabled = false;
        }
    }
}
