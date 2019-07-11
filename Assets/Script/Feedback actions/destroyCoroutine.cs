using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyCoroutine : MonoBehaviour
{
    public float Timer;
    public void Awake()
    {
        StartCoroutine("jemedetruit");
    }
    IEnumerator jemedetruit()
    {
        yield return new WaitForSeconds(Timer);
        Destroy(this.gameObject);
    }
}
