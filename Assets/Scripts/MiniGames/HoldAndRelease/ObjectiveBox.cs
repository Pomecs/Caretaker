using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBox : MonoBehaviour
{

    public int min;
    public int max;

    public bool WithinRange(float value){
        return value > min && value < max;
    }

}
