using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    public float Speed = 3;
    public GameObject Explosion;


    void Update()
    {
        transform.Translate(-transform.right * Time.deltaTime * Speed);
    }

    void OnCollisionEnter()
    {
        RaycastHit hit;
        var direction = transform.position - transform.right;
        if(Physics.Raycast(transform.position,direction,out hit))
        {
            print(hit.collider.name);
            var pos = hit.point;
            Instantiate(Explosion,pos,Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
