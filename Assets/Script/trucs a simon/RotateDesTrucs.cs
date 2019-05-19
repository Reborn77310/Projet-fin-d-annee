using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDesTrucs : MonoBehaviour
{
    public int wantedAxe = 1;
    public float RotateSpeed;

    void Update()
    {
        if (wantedAxe == 0)
        {
            transform.Rotate(RotateSpeed, 0, 0);
        }
        else if (wantedAxe == 1)
        {
            transform.Rotate(0, RotateSpeed, 0);
        }
        else
        {
            transform.Rotate(0, 0, RotateSpeed);
        }
    }
}
