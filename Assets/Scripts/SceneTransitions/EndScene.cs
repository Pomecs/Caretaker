using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{

    public SpriteRenderer backgroundImage;
    // Start is called before the first frame update
    void Start()
    {
     if(GameManager.getFinalScore() < 10){
        backgroundImage.color = Color.black;
     } else {
        backgroundImage.color = Color.blue;
     }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
