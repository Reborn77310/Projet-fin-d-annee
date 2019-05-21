﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;

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
    BattleLog battleLog;
    private CardSound cardSound;
    EnnemiManager ennemiManager;
    Equipement equipement;
    public GameObject uiPlaceHolder;
    public GameObject zoneSelectionADV;
    public Canvas myCanvas;
    public bool isPaused = false;
    public GameObject pause;
    CameraShake cameraShake;

    void Awake()
    {
        cardSound = Camera.main.GetComponent<CardSound>();
        cartesManager = GetComponent<CartesManager>();
        cajSR = caj.GetComponent<SpriteRenderer>();
        salleManager = GetComponent<SalleManager>();
        ennemiManager = GetComponent<EnnemiManager>();
        allSetupsActions = GetComponent<AllSetupsActions>();
        equipement = GetComponent<Equipement>();
        battleLog = GetComponent<BattleLog>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
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
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            for (int i = 0; i < ennemiManager.animators.Length; i++)
            {
                ennemiManager.animators[i].SetBool("hightlight", false);
            }
            for (int i = 0; i < salleManager.animators.Length; i++)
            {
                salleManager.animators[i].SetBool("hightlight", false);
                salleManager.fillCD[i].fillAmount = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPaused)
            {
                cameraShake.UnpauseShake();
                isPaused = false;
                pause.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                cameraShake.PauseShake();
                isPaused = true;
                pause.SetActive(true);
                Time.timeScale = 0;
            }
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
                    ModuleManager mm = hit.transform.gameObject.GetComponent<ModuleManager>();
                    if (mm.cartesModule.Count > 0)
                    {
                        battleLog.ChangeTextInfosNEST(equipement.effets[CheckEquipementSelected(mm)]);
                    }
                    bool canPlay = TestIfCanPlay(hit);

                    if (canPlay)
                    {
                        var moletteSouris = Input.GetAxis("Mouse ScrollWheel");



                        if (mm.cartesModule.Count >= 2)
                        {

                            if (zoneSelectionADV == null)
                            {
                                zoneSelectionADV = GameObject.Instantiate(mm.cartesModule[1].prefabZoneSelection, myCanvas.transform, false);
                                if (equipement.allEquipements[CheckEquipementSelected(mm)].attaque)
                                {
                                    zoneSelectionADV.transform.position = new Vector3(1447, 743, 0);
                                }
                                else
                                {
                                    zoneSelectionADV.transform.position = new Vector3(1647, 293, 0);
                                }

                            }
                            int[] overlapEnnemi;
                            int[] overlapNest;
                            zoneSelectionADV.GetComponent<parent>().RotationZones(mm.MyModules[1].transform);
                            if (equipement.allEquipements[CheckEquipementSelected(mm)].attaque)
                            {
                                overlapEnnemi = zoneSelectionADV.GetComponent<parent>().CheckOverlap(ennemiManager.sallesRT);
                                for (int x = 0; x < ennemiManager.animators.Length; x++)
                                {
                                    if (overlapEnnemi.Contains(x))
                                    {
                                        if (!ennemiManager.animators[x].GetBool("hightlight"))
                                        {
                                            ennemiManager.animators[x].SetBool("hightlight", true);
                                        }
                                    }
                                    else
                                    {
                                        if (ennemiManager.animators[x].GetBool("hightlight"))
                                        {
                                            ennemiManager.animators[x].SetBool("hightlight", false);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                overlapNest = zoneSelectionADV.GetComponent<parent>().CheckOverlap(salleManager.sallesRT);
                                for (int g = 0; g < ennemiManager.animators.Length; g++)
                                {
                                    ennemiManager.animators[g].SetBool("hightlight", false);
                                }
                                for (int g = 0; g < salleManager.fillCD.Length; g++)
                                {
                                    salleManager.fillCD[g].fillAmount = 1;
                                }
                                for (int x = 0; x < salleManager.animators.Length; x++)
                                {
                                    if (overlapNest.Contains(x))
                                    {
                                        if (!salleManager.animators[x].GetBool("hightlight"))
                                        {
                                            salleManager.animators[x].SetBool("hightlight", true);
                                        }
                                    }
                                    else
                                    {
                                        if (salleManager.animators[x].GetBool("hightlight"))
                                        {
                                            salleManager.animators[x].SetBool("hightlight", false);
                                        }
                                    }

                                }
                            }

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


                    if (!cartesManager.CheckHandisFull() && mm.cartesModule.Count > 0)
                    {
                        mm.slotImage[mm.cartesModule.Count - 1].sprite = mm.defaultSprite[mm.cartesModule.Count - 1];

                        if (!CartesManager.PhaseLente)
                        {
                            cartesManager.ModuleToHand(mm);
                            cardSound.CardPickUp();
                        }
                        else
                        {
                            mm.cartesModule.RemoveAt(mm.cartesModule.Count - 1);
                            cardSound.CardPickUp();
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

        int equipementSelected = CheckEquipementSelected(mm);

        string wantedName = equipement.allEquipements[equipementSelected].action;
        if (equipement.allEquipements[equipementSelected].attaque)
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
                if (sallesTouchees[i] >= 0 && ennemiManager.ennemiRooms[sallesTouchees[i]].canBeTarget)
                {
                    if (mm.cartesModule[0].cartesTypes == mm.cartesModule[2].cartesTypes) // CHECK OVERDRIVE
                    {
                        allSetupsActions.FindEffect(wantedName, i, mm, true, equipementSelected);
                    }
                    else
                    {

                        allSetupsActions.FindEffect(wantedName, i, mm, false, equipementSelected);
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
                        allSetupsActions.FindEffect(wantedName, sallesTouchees[i], mm, true, equipementSelected);
                    }
                    else
                    {
                        allSetupsActions.FindEffect(wantedName, sallesTouchees[i], mm, false, equipementSelected);
                    }
                }
            }
        }
    }

    public int CheckEquipementSelected(ModuleManager mm)
    {
        int index = 0;
        if (mm.MySalleNumber == 0 && mm.cartesModule[0].cartesTypes >= 3)
        {
            index = 1;
        }
        else if (mm.MySalleNumber == 1)
        {
            if (mm.cartesModule[0].cartesTypes >= 3)
            {
                index = 4;
            }
            else
            {
                index = 3;
            }

        }
        else if (mm.MySalleNumber == 2)
        {
            if (mm.cartesModule[0].cartesTypes >= 3)
            {
                index = 4;
            }
            else
            {
                index = 5;
            }
        }
        else if (mm.MySalleNumber == 3)
        {
            if (mm.cartesModule[0].cartesTypes >= 3)
            {
                index = 1;
            }
            else
            {
                index = 2;
            }
        }
        return index;

    }
}
