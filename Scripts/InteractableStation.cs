using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStation : MonoBehaviour
{

    private bool inRange = false;
    public GameManager gm;
    public GameObject gameToPlay;
    private bool disabled = false;
    private SpriteRenderer spriteRenderer;
    
    public void Start() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = Color.yellow;
    }

    public void Update() {

        if(Input.GetKeyDown(KeyCode.Space) && inRange && !disabled){
            gm.dodgeDisabled = true;
            gameToPlay?.SetActive(true);
            gm.playerMove = false;
            // afterwards, trigger cooldown
            TriggerCooldown();
        }
        
    }

    public void TriggerCooldown(){
        spriteRenderer.color = Color.red;
        disabled = true;
        StartCoroutine(StopCooldown(5));
    }

    private IEnumerator StopCooldown(int secs){
        yield return new WaitForSeconds(secs);
        spriteRenderer.color = Color.yellow;
        disabled = false;
        // remove this later, this is just here for testing!
        gm.playerMove = true;
        gameToPlay.SetActive(false);
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
            gm.dodgeDisabled = false;
        }
    }
}
