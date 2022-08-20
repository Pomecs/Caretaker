using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    public GameObject[] projectiles;

    void Update()
    {
        if (Input.GetKeyDown("right")){
            GameObject projectile = Instantiate(projectiles[0], transform.position, Quaternion.identity);
            projectile.transform.SetParent(GameObject.Find("FinalBattle").transform);
        } else if(Input.GetKeyDown("up")){
            GameObject projectile = Instantiate(projectiles[1], transform.position, Quaternion.identity);
            projectile.transform.SetParent(GameObject.Find("FinalBattle").transform);
        } else if(Input.GetKeyDown("down")){
            GameObject projectile = Instantiate(projectiles[2], transform.position, Quaternion.identity);
            projectile.transform.SetParent(GameObject.Find("FinalBattle").transform);
        }
    }
}
