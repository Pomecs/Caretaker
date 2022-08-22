using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{

    public GameObject losingScene;
    public GameObject winningScene;
    public SceneTransition sceneTransition;

    // Start is called before the first frame update
    void Start(){
    
     if(GameManager.getScore() < 500){
       losingScene.SetActive(true);
     } else {
        winningScene.SetActive(true);
     }
    }

    // Update is called once per frame
    void Update()
    {

      if(Input.GetKeyDown(KeyCode.Space)){
        sceneTransition.LoadScene("restart");
      }
        
    }
}
