using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnabler : MonoBehaviour
{
    public GameObject tutorial;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.tutorial){
               StartCoroutine(StartTutorial());
        }
     
    }

    public void Quit(){
       Application.Quit();
    }



    IEnumerator StartTutorial() {
        yield return new WaitForSeconds(2);
        tutorial.SetActive(true);
    }

}
