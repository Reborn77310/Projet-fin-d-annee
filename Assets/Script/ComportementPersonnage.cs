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
    public Transform[] ConsolesPositions = new Transform[4];
    public Transform[] CommonPositions = new Transform[4];
    public int NumberOfCategories = 2;

    SalleManager salleManager;
    EnnemiManager ennemiManager;
    void Awake()
    {
        salleManager = GameObject.Find("GameMaster").GetComponent<SalleManager>();
        ennemiManager = GameObject.Find("GameMaster").GetComponent<EnnemiManager>();

        var wantedAgent = GameObject.Find("PersoRouge").GetComponent<NavMeshAgent>();
        var newAgent = new Agents(wantedAgent);
        MyAgents.Add(newAgent);

        wantedAgent = GameObject.Find("PersoBleu").GetComponent<NavMeshAgent>();
        newAgent = new Agents(wantedAgent);
        MyAgents.Add(newAgent);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            print(MyAgents[1]._agent.remainingDistance);
        }
        CheckState();
    }

    void LateUpdate()
    {
        for (int i = 0; i < 2; i++)
        {
            if (MyAgents[i]._agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                MyAgents[i]._agent.transform.rotation = Quaternion.LookRotation(MyAgents[i]._agent.velocity.normalized);
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
                var newDirection = SelectDirection(i);

                if (CheckIfRoomIsPlayable(i) && MyAgents[otherI].destinationGo != MyAgents[i].destinationGo)
                {
                    MyAgents[i]._agent.destination = newDirection;
                    MyAgents[i].etat = 1;
                }
            }
            else if (MyAgents[i].etat == 1) //Va vers la destination
            {
                if (MyAgents[i]._agent.remainingDistance > 0.2f && !MyAgents[i].animator.GetBool("GoRun"))
                {
                    MyAgents[i].animator.SetBool("GoRun", true);
                }
                else if (MyAgents[i]._agent.remainingDistance <= 0.2f)
                {
                    MyAgents[i].animator.SetBool("GoRun", false);
                    MyAgents[i].etat = 2;
                }

                if (!CheckIfRoomIsPlayable(i))
                {
                    MyAgents[i].etat = 0;
                    print("wat");
                }

            }
            else if (MyAgents[i].etat == 2) //Calcul du rand
            {
                MyAgents[i].randomTime = ReturnRandom(10);
                MyAgents[i].etat = 3;
            }
            else if (MyAgents[i].etat == 3) //Arrivé à destination
            {
                MyAgents[i].randomTime -= Time.deltaTime;

                if (MyAgents[i].randomTime <= 0)
                {
                    MyAgents[i].etat = 0;
                }
                else if (!CheckIfRoomIsPlayable(i))
                {
                    MyAgents[i].etat = 0;
                    print("wat1");
                }
            }
        }
    }

    bool CheckIfRoomIsPlayable(int playerSelectedIndex)
    {
        bool canPlayHere = true;
        for (int b = 0; b < ennemiManager.effetsEnnemi.Count; b++)
        {
            string[] stringArray = MyAgents[playerSelectedIndex].destinationGo.name.Split(char.Parse("_"));

            if (ennemiManager.effetsEnnemi[b].salle == int.Parse(stringArray[1]))
            {
                string[] tags = ennemiManager.effetsEnnemi[b].tags;
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
        int random = ReturnRandom(NumberOfCategories);
        var wantedPos = Vector3.zero;

        if (random == 0)
        {
            wantedPos = PickRandomPositionWithArrays(ConsolesPositions, i);
        }
        else if (random == 1)
        {
            wantedPos = PickRandomPositionWithArrays(CommonPositions, i);
        }
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
