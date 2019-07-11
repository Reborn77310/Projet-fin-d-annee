using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    public float Speed = 3;
    public GameObject Explosion;


    void Update()
    {
        if (transform.parent.transform.parent.name == "Salle0" || transform.parent.transform.parent.name == "Salle3")
        {
            transform.Translate(-transform.parent.right * Time.deltaTime * Speed);
        }
        else
        {
            transform.Translate(transform.parent.right * Time.deltaTime * Speed);
        }

    }

    void OnCollisionEnter(Collision col)
    {
        var pos = col.contacts[0].point;
        Instantiate(Explosion, pos, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
