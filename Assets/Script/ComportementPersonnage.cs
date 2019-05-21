using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportementPersonnage : MonoBehaviour
{
    public NavMeshAgent[] agents = new NavMeshAgent[2];
    public Animator[] agentsAnimator = new Animator[2];

    public Transform[] ConsolesPositions = new Transform[4];
    public Transform[] CommonPositions = new Transform[4];
    public int NumberOfCategories = 2;

    Vector3[] agentsDestination = new Vector3[2];
    Transform[] agentsDestinationGo = new Transform[2];

    SalleManager salleManager;
    EnnemiManager ennemiManager;
    void Awake()
    {
        salleManager = GameObject.Find("GameMaster").GetComponent<SalleManager>();
        ennemiManager = GameObject.Find("GameMaster").GetComponent<EnnemiManager>();

        FindFirstDestination();
    }

    void FindFirstDestination()
    {
        agentsDestination[0] = SelectDirection(0);
        agents[0].SetDestination(agentsDestination[0]);

        agentsDestination[1] = SelectDirection(1);
        while (agentsDestination[1] == agentsDestination[0])
        {
            agentsDestination[1] = SelectDirection(1);
        }
        agents[1].SetDestination(agentsDestination[1]);
    }

    void Update()
    {
        CheckIfRunning();
    }

    void CheckIfRunning()
    {
        for (int i = 0; i < agents.Length; i++)
        {
            if (agents[i].remainingDistance > 0.2f && !agentsAnimator[i].GetBool("GoRun"))
            {
                agentsAnimator[i].SetBool("GoRun", true);
            }
            else if (agents[i].remainingDistance <= 0.2f && agentsAnimator[i].GetBool("GoRun"))
            {

                agentsAnimator[i].SetBool("GoRun", false);
                StartCoroutine("StartNewDirection", i);
            }
        }
    }

    IEnumerator StartNewDirection(int i)
    {
        var randomTime = ReturnRandom(10);
        yield return new WaitForSeconds(randomTime);

        int a = 0;
        if (i == 0)
        {
            a = 1;
        }

        agentsDestination[i] = SelectDirection(i);
        while (agentsDestination[i] == agentsDestination[a])
        {
            agentsDestination[i] = SelectDirection(i);
        }
        if (!CheckIfRoomIsPlayable(i))
        {
            StartCoroutine("StartNewDirection", i);
            yield break;
        }
        agents[i].SetDestination(agentsDestination[i]);
        yield break;
    }

    bool CheckIfRoomIsPlayable(int playerSelectedIndex)
    {
        bool canPlayHere = true;
        for (int b = 0; b < ennemiManager.effetsEnnemi.Count; b++)
        {
            string[] stringArray = agentsDestinationGo[playerSelectedIndex].name.Split(char.Parse("_"));


            if (ennemiManager.effetsEnnemi[b].salle == int.Parse(stringArray[1]))
            {
                var tags = ennemiManager.effetsEnnemi[b].tags;
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
        Vector3 toReturn = Vector3.zero;
        int randomPos = ReturnRandom(wantedTransform.Length);

        toReturn = wantedTransform[randomPos].position;
        agentsDestinationGo[i] = wantedTransform[randomPos];

        return toReturn;
    }
}
