using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportementPersonnage : MonoBehaviour
{
    class Agents
    {
        public NavMeshAgent _agent;
        public Animator animator;
        public GameObject destinationGo;
        public int etat;
        public float randomTime;
        public Agents(NavMeshAgent agent)
        {
            _agent = agent;
            animator = _agent.transform.GetChild(1).transform.GetChild(0).GetComponent<Animator>();
            etat = 0;
            randomTime = 0;
            _agent.updateRotation = false;
        }
    }
    List<Agents> MyAgents = new List<Agents>();
    public Transform[] ConsolesPositions = new Transform[5];
    public int NumberOfCategories = 2;

    SalleManager salleManager;
    void Awake()
    {
        salleManager = GameObject.Find("GameMaster").GetComponent<SalleManager>();

        var wantedAgent = GameObject.Find("PersoRouge").GetComponent<NavMeshAgent>();
        var newAgent = new Agents(wantedAgent);
        MyAgents.Add(newAgent);

        wantedAgent = GameObject.Find("PersoBleu").GetComponent<NavMeshAgent>();
        newAgent = new Agents(wantedAgent);
        MyAgents.Add(newAgent);
    }

    void Update()
    {
        CheckState();
    }

    void LateUpdate()
    {
        for (int i = 0; i < 2; i++)
        {
            if (MyAgents[i]._agent.velocity.sqrMagnitude > Mathf.Epsilon && MyAgents[i].etat != 3)
            {
                MyAgents[i]._agent.transform.rotation = Quaternion.LookRotation(MyAgents[i]._agent.velocity.normalized);
            }
            else if (MyAgents[i].etat == 3)
            {
                var positionToLook = (MyAgents[i].destinationGo.transform.position - MyAgents[i]._agent.transform.position).normalized;
                MyAgents[i]._agent.transform.rotation = Quaternion.LookRotation(positionToLook);
            }
        }
    }

    void CheckState()
    {
        for (int i = 0; i < 2; i++)
        {
            var otherI = 0;
            if (i == 0)
            {
                otherI = 1;
            }

            if (MyAgents[i].etat == 0) //Trouve une nouvelle destination
            {
                MyAgents[i].destinationGo = null;
                MyAgents[i].animator.SetBool("GoRun", false);
                MyAgents[i].animator.SetBool("OnConsole", false);
                var newDirection = SelectDirection(i);

                if (CheckIfRoomIsPlayable(i) && MyAgents[otherI].destinationGo != MyAgents[i].destinationGo)
                {
                    MyAgents[i]._agent.destination = newDirection;
                    MyAgents[i].etat = 1;
                }
            }
            else if (MyAgents[i].etat == 1) //Va vers la destination
            {
                if (MyAgents[i]._agent.remainingDistance > 1.2f && !MyAgents[i].animator.GetBool("GoRun"))
                {
                    MyAgents[i].animator.SetBool("GoRun", true);
                }
                else if (MyAgents[i]._agent.remainingDistance <= 1.2f)
                {
                    MyAgents[i].animator.SetBool("GoRun", false);
                    MyAgents[i].etat = 2;
                }

                if (!CheckIfRoomIsPlayable(i))
                {
                    MyAgents[i].etat = 0;
                }

            }
            else if (MyAgents[i].etat == 2) //Calcul du rand
            {
                MyAgents[i].randomTime = Random.Range(5, 10);
                MyAgents[i].etat = 3;
            }
            else if (MyAgents[i].etat == 3) //Arrivé à destination
            {
                MyAgents[i].randomTime -= Time.deltaTime;
                MyAgents[i].animator.SetBool("OnConsole", true);

                if (MyAgents[i].randomTime <= 0)
                {
                    MyAgents[i].etat = 0;
                }
                else if (!CheckIfRoomIsPlayable(i))
                {
                    MyAgents[i].etat = 0;
                }
            }
        }
    }

    bool CheckIfRoomIsPlayable(int playerSelectedIndex)
    {
        bool canPlayHere = true;
        for (int b = 0; b < salleManager.allEffets.Count; b++)
        {
            string[] stringArray = MyAgents[playerSelectedIndex].destinationGo.name.Split(char.Parse("_"));

            if (salleManager.allEffets[b].salle == int.Parse(stringArray[1]))
            {
                string[] tags = salleManager.allEffets[b].tags;
                for (int c = 0; c < tags.Length; c++)
                {
                    if (tags[c] == "CHIMIC" || tags[c] == "DOT")
                    {
                        canPlayHere = false;
                    }
                }
            }
        }
        return canPlayHere;
    }
    Vector3 SelectDirection(int i)
    {    
        var wantedPos = Vector3.zero;

        wantedPos = PickRandomPositionWithArrays(ConsolesPositions, i);

        return wantedPos;
    }

    int ReturnRandom(int wantedRandomValue)
    {
        int toReturn = 0;
        toReturn = Random.Range(0, wantedRandomValue);
        return toReturn;
    }

    Vector3 PickRandomPositionWithArrays(Transform[] wantedTransform, int i)
    {
        Transform toReturn;
        int randomPos = ReturnRandom(wantedTransform.Length);

        toReturn = wantedTransform[randomPos];
        while (MyAgents[i].destinationGo == wantedTransform[randomPos])
        {
            randomPos = ReturnRandom(wantedTransform.Length);
            toReturn = wantedTransform[randomPos];
        }
        MyAgents[i].destinationGo = wantedTransform[randomPos].gameObject;

        return toReturn.position;
    }
}
