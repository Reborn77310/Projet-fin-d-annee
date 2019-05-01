﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWithDécalage : MonoBehaviour {

    public Animator[] AllLegs;
    public float Timer = 0;
    public float[] timebetweenlegs;

    // Use this for initialization
    void Start ()
    {
        AllLegs = GameObject.FindObjectsOfType<Animator>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        Timer += 1 * Time.deltaTime;
        //JambeAvantGauche
        if (Timer > timebetweenlegs[0])
        {
            AllLegs[0].SetBool("AnimGo", true);
            Debug.Log("JambeAvantGauche");


        }
        //JambeArrèreGauche
        if (Timer > timebetweenlegs[1])
        {
            AllLegs[1].SetBool("AnimGo", true);
            Debug.Log("JambeArrèreGauche");
        }
        //JambeAvantDroite
        if (Timer > timebetweenlegs[2])
        {
            AllLegs[2].SetBool("AnimGo", true);
            Debug.Log("JambeAvantDroite");
        }
        //JambeArrièreDroite
        if (Timer > timebetweenlegs[3])
        {
            AllLegs[3].SetBool("AnimGo", true);
            Debug.Log("JambeArrièreDroite");
        }
    }
}
