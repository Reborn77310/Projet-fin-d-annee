using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWithDécalage : MonoBehaviour
{

    public Animator[] AllLegs;
    public float Timer = 0;
    public float[] timebetweenlegs;

    public static bool CanMove = false;

    // Use this for initialization
    void Start()
    {
        //AllLegs = GameObject.FindObjectsOfType<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            Timer += 1 * Time.deltaTime;
            //JambeAvantGauche
            if (Timer > timebetweenlegs[0])
            {
                AllLegs[0].SetBool("AnimGo", true);


            }
            //JambeArrèreGauche
            if (Timer > timebetweenlegs[1])
            {
                AllLegs[1].SetBool("AnimGo", true);
            }
            //JambeAvantDroite
            if (Timer > timebetweenlegs[2])
            {
                AllLegs[2].SetBool("AnimGo", true);
            }
            //JambeArrièreDroite
            if (Timer > timebetweenlegs[3])
            {
                AllLegs[3].SetBool("AnimGo", true);
            }
        }
        else
        {
            for(int i =0; i< AllLegs.Length; i++)
            {
                AllLegs[i].SetBool("AnimGo", false);
            }
        }
    }
}
