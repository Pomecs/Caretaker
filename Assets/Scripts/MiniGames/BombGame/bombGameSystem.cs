using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombGameSystem : MonoBehaviour
{
    public int MAX_POSITION = 47;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float randomSpeed = Random.Range(2f, 6f);

        if(transform.position.x >= MAX_POSITION){
            transform.position = new Vector3(-MAX_POSITION, 0, 0);
        }

        transform.Translate(randomSpeed * Time.deltaTime, 0, 0);
    }
}
