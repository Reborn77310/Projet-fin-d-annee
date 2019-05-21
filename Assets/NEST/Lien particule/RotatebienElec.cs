using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatebienElec : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent.transform.parent.name == "Salle0" || transform.parent.transform.parent.name == "Salle4")
        {
            transform.Rotate(-90,0,0);
        }
        else{
            transform.Rotate(90,0,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
