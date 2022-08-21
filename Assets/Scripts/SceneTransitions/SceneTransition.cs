using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator transitionAnim;
    private void Start() {
        Cursor.visible = false;
        transitionAnim = GetComponent<Animator>();
    }

    public void LoadScene(string sceneName){
        StartCoroutine(Transition(sceneName));
    }

    public void Quit(){
       Application.Quit();
    }



    IEnumerator Transition(string sceneName) {
        
        transitionAnim.SetTrigger("end");
        if(sceneName == "Lose"){
            yield return new WaitForSeconds(3);   
        }

        if(sceneName == "restart"){
            GameManager.tutorial = false;
            sceneName = "Game";
        }

        


        
    yield return new WaitForSeconds(1);
       SceneManager.LoadScene(sceneName);
    }
}
