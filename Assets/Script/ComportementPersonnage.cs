using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportementPersonnage : MonoBehaviour
{
    public NavMeshAgent[] agents = new NavMeshAgent[2];
    public Animator[] agentsAnimator = new Animator[2];
    void Awake()
    {
        agents[0].SetDestination(new Vector3(-25.32268f,-2.87211f,-9.4f));
        agents[1].SetDestination(new Vector3(-13.52f,-2.87211f,5.442418f));
    }

    public void CheckState()
    {
        for(int i =0; i < agents.Length; i++)
        {

        }
    }
}
