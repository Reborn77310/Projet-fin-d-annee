using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gaz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(transform.parent.transform.parent);
        var pos = Vector3.zero;
        transform.Rotate(-90,0,0);
        pos.y = 0.7f;
        transform.localPosition = pos;        
    }
}
