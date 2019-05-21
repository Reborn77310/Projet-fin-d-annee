using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class position : MonoBehaviour
{
    void Start()
    {        
        var parent = transform.parent;
        var index = parent.transform.parent.GetComponent<ModuleManager>().MySalleNumber;
        var positionToGet = GameObject.Find("Console" + index).transform.position;
        transform.position = positionToGet;
    }
}
