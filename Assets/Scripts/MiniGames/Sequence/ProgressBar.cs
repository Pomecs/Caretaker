using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    private Slider bar;
    public float speed;
    private int maxBar = 100;
    private int currentBar;
    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<Slider>();
        currentBar = maxBar;
        bar.maxValue = maxBar;
        bar.value = 50;

    }
    

    private void OnEnable(){
        
        bar = GetComponent<Slider>();
        currentBar = maxBar;
        bar.maxValue = maxBar;
        bar.value = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if(bar.value == 0){
            Debug.Log("YOU LOSE");
            // close
            FindObjectOfType<SequenceGame>().Disable();
        } 

        if(bar.value == maxBar){
            if(GameManager.isPlayerAtRightStation){
                GameManager.increaseScore(10);
            }

            FindObjectOfType<SequenceGame>().Disable();
        }

        bar.value -= Time.deltaTime * speed;
    }

    public void IncreaseValue(){
        bar.value += 5;
    }  
}
