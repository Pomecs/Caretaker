using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    
    public GameObject next;
    private Animator anim;
    public bool first;

    private void OnEnable() {
       
        if(first){ 
            anim = GetComponent<Animator>();
            anim.SetTrigger("popup");
        }
        
    }

    private void Update() {

        if(Input.GetKeyDown(KeyCode.Space)){
            if(next != null){
                next.SetActive(true);
            }
            gameObject.SetActive(false);
        }    
    }
}
