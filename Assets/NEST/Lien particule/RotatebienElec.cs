using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatebienElec : MonoBehaviour
{
 
    void Start()
    {
        

        if (transform.parent.transform.parent.name == "Salle0" || transform.parent.transform.parent.name == "Salle4")
        {
            transform.Rotate(-90, 0, 0);
        }
        else
        {
            transform.Rotate(90, 0, 0);
        }

        GameObject.Find("GameMaster").GetComponent<SonLASER>().playlaster();
        StartCoroutine("Destroy");
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
