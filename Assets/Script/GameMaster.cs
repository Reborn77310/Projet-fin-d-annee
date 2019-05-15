using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    Camera cam;

    public Vector3 offset;

    public static bool cursorIsOnCard = false;
    public static bool isPlayingACard = false;
    public static bool hittingAModule = false;
    public static bool endPlayingCard = false;

    public static int cardIDBeingPlayed = -1;

    public static ModuleManager moduleHit;
    public GameObject caj;
    SpriteRenderer cajSR;
    CartesManager cartesManager;
    SalleManager salleManager;
    AllSetupsActions allSetupsActions;
    private CardSound cardSound;
    EnnemiManager ennemiManager;
    public GameObject uiPlaceHolder;
    public GameObject zoneSelectionADV;
    public Canvas myCanvas;


    void Awake()
    {
        cardSound = Camera.main.GetComponent<CardSound>();
        cartesManager = GetComponent<CartesManager>();
        cajSR = caj.GetComponent<SpriteRenderer>();
        salleManager = GetComponent<SalleManager>();
        ennemiManager = GetComponent<EnnemiManager>();
    }

    void Update()
    {


        RotateSecondModule();
        if (isPlayingACard)
        {
            PlayerLine();
        }
        else if (endPlayingCard)
        {
            if (hittingAModule)
            {
                PlayCard();
            }
            hittingAModule = false;
            endPlayingCard = false;
        }
        else
        {
            if (cajSR.enabled)
            {
                cajSR.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            TakeCardFromModule();
        }
    }

    #region Actions

    void RotateSecondModule()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            var origin = new Vector3(2.3f, -2.095f, -16.11f);
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));


            RaycastHit hit;
            var dist = Vector3.Distance(origin, point);
            var dir = (point - Camera.main.transform.position);

            caj.transform.position = point + dir;

            if (Physics.Raycast(caj.transform.position, dir, out hit, dist))
            {
                if (hit.transform.tag == "Salles")
                {
                    bool canPlay = TestIfCanPlay(hit);

                    if (canPlay)
                    {
                        var moletteSouris = Input.GetAxis("Mouse ScrollWheel");
                        ModuleManager mm = hit.transform.gameObject.GetComponent<ModuleManager>();


                        if (mm.cartesModule.Count >= 2)
                        {

                            if (zoneSelectionADV == null)
                            {
                                zoneSelectionADV = GameObject.Instantiate(mm.cartesModule[1].prefabZoneSelection, myCanvas.transform, false);
                                if (salleManager.allSalles[mm.MySalleNumber].equipementATK[0])
                                {
                                    zoneSelectionADV.transform.position = new Vector3(1447, 744, 0);
                                }
                                else
                                {
                                    zoneSelectionADV.transform.position = new Vector3(1647, 292, 0);
                                }

                            }

                            zoneSelectionADV.GetComponent<parent>().RotationZones(mm.MyModules[1].transform);
                            if (salleManager.allSalles[mm.MySalleNumber].equipementATK[0])
                            {
                                zoneSelectionADV.GetComponent<parent>().CheckOverlap(ennemiManager.sallesRT);
                            }
                            else
                            {
                                zoneSelectionADV.GetComponent<parent>().CheckOverlap(salleManager.sallesRT);
                            }


                            // ADD else sur nest

                        }

                        if (Mathf.Abs(moletteSouris) > 0)
                        {
                            mm.MyModules[1].transform.Rotate(0, 0, -90 * (moletteSouris * 10));
                            if (moletteSouris < 0 && mm.rotationCompteur == 0)
                            {
                                mm.rotationCompteur = 400000;
                            }
                            mm.rotationCompteur += 1 * (Mathf.RoundToInt(moletteSouris * 10));
                        }
                    }
                }
                else
                {
                    Destroy(zoneSelectionADV);
                    zoneSelectionADV = null;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && zoneSelectionADV != null)
        {
            Destroy(zoneSelectionADV);
            zoneSelectionADV = null;
        }
    }

    public void TakeCardFromModule()
    {
        var origin = new Vector3(2.3f, -2.095f, -16.11f);
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));


        RaycastHit hit;
        var dist = Vector3.Distance(origin, point);
        var dir = (point - Camera.main.transform.position);

        caj.transform.position = point + dir;

        if (Physics.Raycast(caj.transform.position, dir, out hit, dist))
        {
            if (hit.transform.tag == "Salles")
            {
                bool canPlay = TestIfCanPlay(hit);

                if (canPlay)
                {
                    ModuleManager mm = hit.transform.GetComponent<ModuleManager>();


                    if (!cartesManager.CheckHandisFull())
                    {
                        mm.MyModules[mm.cartesModule.Count - 1].transform.GetComponent<Image>().sprite =
                            Resources.Load<Sprite>("MiniUi/CadresCartes_0");

                        if (!CartesManager.PhaseLente)
                        {
                            cartesManager.ModuleToHand(mm);
                        }
                        else{
                            mm.cartesModule.RemoveAt(mm.cartesModule.Count -1);
                        }

                    }
                }
            }
        }
    }




    public void PlayerLine()
    {
        if (cajSR.enabled == false)
        {
            cajSR.enabled = true;
            caj.GetComponent<Animator>().SetTrigger("Pressed");
        }
        var origin = new Vector3(2.3f, -2.095f, -16.11f);
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));


        RaycastHit hit;
        var dist = Vector3.Distance(origin, point);
        var dir = (point - Camera.main.transform.position);

        caj.transform.position = point + dir;

        if (Physics.Raycast(caj.transform.position, dir, out hit, dist))
        {
            if (hit.transform.tag == "Salles")
            {
                bool canPlay = TestIfCanPlay(hit);

                if (canPlay)
                {
                    caj.GetComponent<Animator>().SetBool("Dance", true);
                    hittingAModule = true;
                    moduleHit = hit.transform.GetComponent<ModuleManager>();

                }
            }
            else
            {
                if (moduleHit != null)
                {
                    moduleHit = null;
                    hittingAModule = false;
                    caj.GetComponent<Animator>().SetBool("Dance", false);
                }
            }
        }
        else
        {

            if (moduleHit != null)
            {
                moduleHit = null;
                hittingAModule = false;
                caj.GetComponent<Animator>().SetBool("Dance", false);
            }
        }
    }

    public bool TestIfCanPlay(RaycastHit hit)
    {
        bool toReturn = true;

        for (int i = 0; i < salleManager.allSalles.Count; i++)
        {
            if (hit.transform.gameObject == salleManager.allSalles[i].MyGo)
            {
                if (!salleManager.allSalles[i].CanPlayHere)
                {
                    toReturn = false;
                }
            }
        }

        return toReturn;
    }
    public void PlayCard()
    {
        cartesManager.PlayACardOnModule(cardIDBeingPlayed, moduleHit);
        cardSound.GoingToPlayACard();
        cardIDBeingPlayed = -1;
    }
    #endregion

    public void LancerActionJoueur(ModuleManager mm)
    {
        // Check equipement selectionné par 1e module
        // Check salles touchées par 2nd module
        // Check Overdrive par 3e module
        // Lancer action
        // Burn 3e carte
        // Check durabilité => Burn 2e carte

        string wantedName = salleManager.allSalles[mm.MySalleNumber].equipement[0];
        if (salleManager.allSalles[mm.MySalleNumber].equipementATK[0])
        {
            zoneSelectionADV = GameObject.Instantiate(mm.cartesModule[1].prefabZoneSelection, myCanvas.transform, false);
            zoneSelectionADV.transform.position = new Vector3(1447, 744, 0);
            parent overlapScript = zoneSelectionADV.GetComponent<parent>();
            overlapScript.RotationZones(mm.MyModules[1].transform);
            var sallesTouchees = overlapScript.CheckOverlap(ennemiManager.sallesRT);
            Destroy(zoneSelectionADV);
            zoneSelectionADV = null;

            for (int i = 0; i < sallesTouchees.Length; i++)
            {
                if (sallesTouchees[i] >= 0)
                {
                    if (mm.cartesModule[0].cartesTypes == mm.cartesModule[2].cartesTypes) // CHECK OVERDRIVE
                    {
                        allSetupsActions.FindEffect(wantedName, sallesTouchees[i], mm, true);
                    }
                    else
                    {
                        allSetupsActions.FindEffect(wantedName, sallesTouchees[i], mm, false);
                    }

                }
            }

        }
        else
        {
            zoneSelectionADV = GameObject.Instantiate(mm.cartesModule[1].prefabZoneSelection, myCanvas.transform, false);
            zoneSelectionADV.transform.position = new Vector3(1647, 292, 0);
            parent overlapScript = zoneSelectionADV.GetComponent<parent>();
            overlapScript.RotationZones(mm.MyModules[1].transform);
            var sallesTouchees = overlapScript.CheckOverlap(salleManager.sallesRT);
            Destroy(zoneSelectionADV);
            zoneSelectionADV = null;

            for (int i = 0; i < sallesTouchees.Length; i++)
            {
                if (sallesTouchees[i] >= 0)
                {
                    if (mm.cartesModule[0].cartesTypes == mm.cartesModule[2].cartesTypes) // CHECK OVERDRIVE
                    {
                        allSetupsActions.FindEffect(wantedName, sallesTouchees[i], mm, true);
                    }
                    else
                    {
                        allSetupsActions.FindEffect(wantedName, sallesTouchees[i], mm, false);
                    }
                }
            }
        }

        mm.cartesModule.RemoveAt(2);
        mm.MyCompteurInt -= 1;
        mm.MyCompteur.text = mm.MyCompteurInt.ToString();

        if (mm.MyCompteurInt <= 0)
        {
            mm.cartesModule.RemoveAt(1);
        }
    }
}
