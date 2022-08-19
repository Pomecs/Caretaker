using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldAndRelease : MonoBehaviour
{

    private Slider bar;
    public float speed;
    private int maxBar = 100;
    private int currentBar;
    private bool pressed;

    public GameObject gameOb;
    private ObjectiveBox ob;
    
    private void OnEnable(){
        bar = GetComponentInChildren<Slider>();
        ob = gameOb.GetComponent<ObjectiveBox>();
        Debug.Log(bar);
        currentBar = maxBar;
        bar.maxValue = maxBar;
        bar.value = 0;
    }


   private void Update() {
    
        if(Input.GetKeyDown(KeyCode.DownArrow) && !pressed){
            pressed = true;
        } 

        if(pressed){
            bar.value += speed;
        }

        if(Input.GetKeyUp(KeyCode.DownArrow)){
            Debug.Log("finished!");
            pressed = false;
            if(ob.WithinRange(bar.value)){
                // increase score
                GameManager.increaseScore(10);
                Debug.Log("Good job!");
                
            }
            gameObject.SetActive(false);
            GameManager.playerMove = true;
        }

        
   }

}