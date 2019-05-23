﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;

public class GameMaster : MonoBehaviour
{
    public Texture2D cursorBase;
    public Texture2D cursorClic;
    public int etat = -1;
    Vector3 PhaseLenteDestination = Vector3.zero;
    public Transform SecondCam;
    public GameObject perso1;
    public GameObject perso2;
    public GameObject BoutonsDestination;
    public GameObject DeuxiemeDestination;
    public GameObject NeigeTemporaire;
    public GameObject EcranDeSucces;

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
    public CardSound cardSound;
    EnnemiManager ennemiManager;
    Equipement equipement;
    public GameObject uiPlaceHolder;
    public GameObject zoneSelectionADV;
    public Canvas myCanvas;
    public bool isPaused = false;
    public GameObject pause;
    CameraShake cameraShake;
    public Animator[] consolesAnim;

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

    public void FirstDestinationButtonClicked()
    {
        SecondCam.GetComponent<Animator>().SetTrigger("1");
        BoutonsDestination.SetActive(false);

        Color a = GameObject.Find("PictosCartes_3").GetComponent<SpriteRenderer>().color;
        a.a = 100;
        GameObject.Find("PictosCartes_3").GetComponent<SpriteRenderer>().color = a;
        Camera.main.GetComponent<MusiqueScript>().LancerPhaseLente();
        var emission = NeigeTemporaire.GetComponent<ParticleSystem>().emission;
        emission.enabled = true;

        PlayWithDécalage.CanMove = true;
        etat = 1;
    }

    public void FindDestination2()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject.Find("PattesController").GetComponent<PlayWithDécalage>().AllLegs[i].speed = 0;
        }
        var emission = NeigeTemporaire.GetComponent<ParticleSystem>().emission;
        emission.enabled = true;

        BoutonsDestination.SetActive(false);
        Color a = GameObject.Find("PictosCartes_4").GetComponent<SpriteRenderer>().color;
        a.a = 100;
        GameObject.Find("PictosCartes_4").GetComponent<SpriteRenderer>().color = a;
        etat = 3;
    }

    public void RetourAlmanac()
    {
        etat = 4;
        SecondCam.GetComponent<Animator>().SetTrigger("2");
        GameObject.Find("PictosCartes_4").transform.Rotate(0,0,-70.704f);
    }
    public void SuccesGainCarte()
    {
        EcranDeSucces.SetActive(false);
        GameObject.Find("PersoTransmission").GetComponent<VideoTransmission>().Activevideo();        
    }
    public Texture2D cursorTexture;

    void OnMouseEnter()
    {
        Cursor.SetCursor(cursorClic, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(cursorBase, new Vector2(5, 0), CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnMouseEnter();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnMouseExit();
        }
        if (CartesManager.PhaseLente)
        {
            if (etat == -1)
            {
                //Pas de bras qui bouge
                //Pas de "neige"
                //Camera start en dezoom
                //Phase lente = true
                //Persos en IDDLE quelque part
            }
            else if (etat == 1)
            {
                GameObject.Find("PictosCartes_3").transform.rotation = SecondCam.rotation;
                if (Input.GetKeyDown(KeyCode.A))
                {
                    var emission = NeigeTemporaire.GetComponent<ParticleSystem>().emission;
                    emission.enabled = false;

                    for (int i = 0; i < 4; i++)
                    {
                        GameObject.Find("PattesController").GetComponent<PlayWithDécalage>().AllLegs[i].speed = 0;
                    }

                    perso1.GetComponent<MovieTexturePersoUn>().Activevideo();
                    Camera.main.GetComponent<MusiqueScript>().StopPhaseLente();
                }
            }
            else if (etat == 2)
            {
                GameObject.Find("TransmissionCadre").SetActive(false);
                BoutonsDestination.SetActive(true);
                Destroy(GameObject.Find("PictosCartes_3"));
                Destroy(BoutonsDestination.transform.GetChild(3).gameObject);
                BoutonsDestination.GetComponent<BoutonsScript>().Buttons[3] = null;
                DeuxiemeDestination.SetActive(true);
                etat = -1;
            }
            else if (etat == 3)
            {
                //On attend que le joueur fasse ses bails dans l'almanac

            }
            else if (etat == 4)
            {
                SecondCam.GetComponent<Animator>().SetTrigger("2");
                etat = 5;
            }
            else if (etat == 5)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    PlayWithDécalage.CanMove = false;
                    perso2.GetComponent<MovieTexturePersoDeux>().Activevideo();
                }
            }
        }
        else
        {
            UpdatePasPhaseLente();
        }
    }

    void UpdatePasPhaseLente()
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
                if (ennemiManager.badGuy != null)
                {
                    ennemiManager.animators[i].SetBool("hightlight", false);
                }
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


                        cartesManager.ModuleToHand(mm);
                        cardSound.CardPickUp();

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
                    consolesAnim[moduleHit.MySalleNumber].SetBool("hover", true);
                }
            }
            else
            {
                if (moduleHit != null)
                {
                    consolesAnim[moduleHit.MySalleNumber].SetBool("hover", false);
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
                consolesAnim[moduleHit.MySalleNumber].SetBool("hover", false);
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
        consolesAnim[moduleHit.MySalleNumber].SetBool("cooldown", true);
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
                if (ennemiManager.badGuy != null)
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

    public void CombatEnded()
    {
        ennemiManager.enabled = false;
        ennemiManager.enabled = true;
        ennemiManager.combat.SetActive(false);
        ennemiManager.horsCombat.SetActive(true);
        ennemiManager.NEST.SetActive(false);

        CartesManager.PhaseLente = true;
        for (int i = 0; i < cartesManager.allCards.Count; i++)
        {
            Destroy(cartesManager.allCards[i].go);
        }
        cartesManager.allCards.Clear();

        Camera.main.GetComponent<Animator>().SetTrigger("GoUp");
        for (int i = 0; i < 4; i++)
        {
            for (int h = 0; h < salleManager.allSalles[i].SpawnParticleFeedback.transform.childCount; h++)
            {
                Destroy(salleManager.allSalles[i].SpawnParticleFeedback.transform.GetChild(h).gameObject);
            }
            for (int j = 0; j < salleManager.allSalles[i].MyGo.transform.childCount; j++)
            {
                if (salleManager.allSalles[i].MyGo.transform.GetChild(j).name == "9(Clone)")
                {
                    Destroy(salleManager.allSalles[i].MyGo.transform.GetChild(j).gameObject);
                }
            }
            consolesAnim[i].SetBool("hover", false);
            consolesAnim[i].SetBool("cooldown", false);
        }
        cajSR.enabled = false;

        etat = -1;
        EcranDeSucces.SetActive(true);
    }
}
