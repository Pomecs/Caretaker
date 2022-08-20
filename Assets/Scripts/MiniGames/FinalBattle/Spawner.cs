using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float startTimeBtwSpawns;
    public GameObject[] enemies;
    private float timeBtwSpawn;
    void Update()
    {
        if(timeBtwSpawn <= 0){
            int randomEnemy = Random.Range(0, enemies.Length);
            GameObject enemy = Instantiate(enemies[randomEnemy], transform.position, Quaternion.identity);
            enemy.transform.SetParent(GameObject.Find("FinalBattle").transform);
            timeBtwSpawn = startTimeBtwSpawns;
        } else {
            timeBtwSpawn -= Time.deltaTime;
        }
    }
}
