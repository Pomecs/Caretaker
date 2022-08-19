using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombGameSystem : MonoBehaviour
{
    public int MAX_POSITION = 47;
    public GameObject bomb;
    private RectTransform rectTransform;

    void Awake(){
        rectTransform = bomb.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float randomSpeed = Random.Range(5f, 15f);
        Vector2 position = rectTransform.anchoredPosition;

        if(position.x >= MAX_POSITION){
            position = new Vector2(-MAX_POSITION, 0);
            Debug.Log($"new position: {rectTransform.anchoredPosition}");
        }

        Debug.Log($"position: {rectTransform.anchoredPosition}");
        position.x += randomSpeed * Time.deltaTime;

        rectTransform.anchoredPosition = position;
    }
}
