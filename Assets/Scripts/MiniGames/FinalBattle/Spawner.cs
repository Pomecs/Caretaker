using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float startTimeBtwSpawns;
    public GameObject[] enemies;
    private float timeBtwSpawn;
    private static List<GameObject> enemiesSpawned = new List<GameObject>();

    void Update()
    {
        if(timeBtwSpawn <= 0 && FinalBattleManager.running){
            int randomEnemy = Random.Range(0, enemies.Length);
            GameObject enemy = Instantiate(enemies[randomEnemy], transform.position, Quaternion.identity);
            enemy.transform.SetParent(GameObject.Find("FinalBattle").transform);
            enemiesSpawned.Add(enemy);
            timeBtwSpawn = startTimeBtwSpawns;
        } else {
            timeBtwSpawn -= Time.deltaTime;
        }
    }

    public static void removeEnemy(GameObject enemy){
        GameObject oldEnemy = enemiesSpawned.Find(x => GameObject.ReferenceEquals(x, enemy));
        oldEnemy?.SetActive(false);
        enemiesSpawned.Remove(enemy);
    }

    public static void removeAllEnemies(){
        foreach(GameObject enemy in enemiesSpawned){
            enemy?.SetActive(false);
        }
        enemiesSpawned.Clear();
    }
}
