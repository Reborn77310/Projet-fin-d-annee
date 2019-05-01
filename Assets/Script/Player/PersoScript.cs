using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PersoScript : MonoBehaviour
{
    /*NavMeshAgent nma;
    public int monID;
    public bool vaJouerUneCarte = false;
    public bool vaRamasserUneCarte = false;
    public bool vaSurUneConsole = false;
    public int carteID = -1;
    public bool isConsoling = false;
    public ConsoleScript myConsole = null;
    public Animator PlayerAnim;
    CartesManager cartesManager;
    private CardSound cardSound;
    public GameObject WantedConsole;
    public int WantedCarteId = -1;
    //GameMaster gameMaster;
    public GameObject lumierePerso;
    public bool canReceiveOrder = true;

    void Start()
    {
        lumierePerso = transform.GetChild(0).gameObject;
        nma = GetComponent<NavMeshAgent>();
        cartesManager = GameObject.Find("GameMaster").GetComponent<CartesManager>();
        cardSound = Camera.main.GetComponent<CardSound>();
        //gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
    }

    void Update()
    {
        if (vaJouerUneCarte)
        {
            JouerUneCarte(carteID);
            if (myConsole.persoOnMeID != monID && myConsole.persoOnMe)
            {
                CancelOrder();
                nma.SetDestination(transform.position);
            }
        }
        else if (vaRamasserUneCarte)
        {
            RamasserUneCarte();
        }
        else if (vaSurUneConsole)
        {
            AllerSurUneConsole();
            if (myConsole.persoOnMeID != monID && myConsole.persoOnMe)
            {
                CancelOrder();
                nma.SetDestination(transform.position);
            }
        }
        DirectionFacing();
    }

    public void JouerUneCarte(int cardID)
    {
        var remainingDistance = Vector3.Distance(transform.position, nma.destination);
        if (remainingDistance < 1.2f)
        {
            StartCoroutine("CoroutineForTeleportation", nma.destination);
            myConsole.persoOnMe = true;
            myConsole.persoOnMeID = monID;
            PlayerAnim.SetBool("GoRun", false);
            PlayerAnim.SetTrigger("PlayCard");
            //StartCoroutine("WaitForIconDisplay");
            cartesManager.PlayACardOnModule(carteID, monID);
            carteID = -1;
            vaJouerUneCarte = false;
            isConsoling = true;
            StartCoroutine("CoroutineForLookingAt", myConsole.keyboardConsoleToLookAt.transform.position);
        }
        else
        {
            if (!PlayerAnim.GetBool("GoRun"))
            {
                PlayerAnim.SetBool("GoRun", true);
            }
        }
    }

    public void RamasserUneCarte()
    {
        var remainingDistance = Vector3.Distance(transform.position, nma.destination);
        if (remainingDistance < 3f)
        {
            StartCoroutine("CoroutineForLookingAt", nma.destination);
            nma.destination = transform.position;
            PlayerAnim.SetBool("GoRun", false);
            PlayerAnim.SetTrigger("Grabbing");
            StartCoroutine("WaitForPickup");
            vaRamasserUneCarte = false;
        }
    }

    public void AllerSurUneConsole()
    {
        var remainingDistance = Vector3.Distance(transform.position, nma.destination);
        if (remainingDistance < 1.2f)
        {
            StartCoroutine("CoroutineForTeleportation", nma.destination);
            myConsole.persoOnMe = true;
            myConsole.persoOnMeID = monID;
            PlayerAnim.SetBool("GoRun", false);
            PlayerAnim.SetBool("OnConsole", true);
            vaSurUneConsole = false;
            isConsoling = true;
            //nma.SetDestination(transform.position);
            StartCoroutine("CoroutineForLookingAt", myConsole.keyboardConsoleToLookAt.transform.position);
        }
    }

    public void CancelOrder()
    {
        if (myConsole != null)
        {
            myConsole.persoOnMe = false;
            myConsole.persoOnMeID = -1;
            myConsole = null;
        }
        PlayerAnim.SetBool("OnConsole", false);
        PlayerAnim.SetBool("GoRun", false);
        vaJouerUneCarte = false;
        vaRamasserUneCarte = false;
        vaSurUneConsole = false;
        isConsoling = false;
        WantedCarteId = -1;
    }


    public void DirectionFacing()
    {
        if (nma.velocity.magnitude > 0.5f)
        {
            Vector3 relativePos = nma.velocity.normalized;
            transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        }
    }

    public void OrderGoToConsole(ConsoleScript _myConsole)
    {
        myConsole = _myConsole;
        PlayerAnim.SetBool("GoRun", true);
        vaSurUneConsole = true;
    }

    public void OrderGoPlayACard(int _cardId, ConsoleScript _myConsole)
    {
        carteID = _cardId;
        myConsole = _myConsole;
        vaJouerUneCarte = true;
    }

    public void OrderGoGetACard(int _wantedCardId)
    {
        WantedCarteId = _wantedCardId;
        PlayerAnim.SetBool("GoRun", true);
        vaRamasserUneCarte = true;
    }

    IEnumerator WaitForPickup()
    {
        yield return new WaitForSeconds(1f);
        cartesManager.AjouterUneCarteDansLaMain(monID, WantedCarteId);
        cardSound.CardPickUp();
    }

    IEnumerator WaitForIconDisplay()
    {
        yield return new WaitForSeconds(0.5f);
        cartesManager.PlayACardOnModule(carteID, monID);
        carteID = -1;
    }
    private void TeleportDestintion(Vector3 _wantedDestination)
    {
        transform.position = Vector3.Lerp(transform.position, _wantedDestination, 0.1f);
    }

    IEnumerator CoroutineForTeleportation(Vector3 _wantedDestination)
    {
        while (true)
        {
            var distanceRemaining = Mathf.Abs((transform.position - _wantedDestination).magnitude);
            TeleportDestintion(_wantedDestination);
            if (distanceRemaining < 0.2f)
            {
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void LookDestination(Vector3 _GameobjectToLook)
    {
        Vector3 relativePos = _GameobjectToLook - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos), 0.75f);
    }

    IEnumerator CoroutineForLookingAt(Vector3 _GameobjectToLook)
    {
        while (true)
        {
            Vector3 relativePos = _GameobjectToLook - transform.position;
            var angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(relativePos));
            LookDestination(_GameobjectToLook);
            if (Mathf.Abs(angle) <= 3f)
            {
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }*/

}
