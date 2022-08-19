using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceGame : MonoBehaviour
{

    private ProgressBar progressBar;
    private KeyCode previousKey = KeyCode.None;
 
    // Start is called before the first frame update
    void Start()
    {
        progressBar = GetComponentInChildren<ProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {

        if(previousKey != KeyCode.None){
            if(Input.GetKeyDown(KeyCode.LeftArrow) && previousKey != KeyCode.LeftArrow){
                progressBar.IncreaseValue();
                previousKey = KeyCode.LeftArrow;
            } else if(Input.GetKeyDown(KeyCode.RightArrow) && previousKey != KeyCode.RightArrow){
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
