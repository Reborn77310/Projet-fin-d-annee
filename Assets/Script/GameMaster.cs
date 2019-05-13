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

    public static GameObject moduleHit;
    public GameObject caj;
    SpriteRenderer cajSR;
    CartesManager cartesManager;
    SalleManager salleManager;
    AllSetupsActions allSetupsActions;
    private CardSound cardSound;
    private GameObject touchedByZoomRaycast;
    EnnemiManager ennemiManager;
    public GameObject uiPlaceHolder;
    public GameObject zoneSelectionADV;


    void Awake()
    {
        cardSound = Camera.main.GetComponent<CardSound>();
        cartesManager = GetComponent<CartesManager>();
        cajSR = caj.GetComponent<SpriteRenderer>();
        salleManager = GetComponent<SalleManager>();
    }

    void Update()
    {


        placeHolderUICiblage();
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

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Module")
            {
                touchedByZoomRaycast = hit.transform.gameObject;
                var text = hit.transform.GetChild(1).GetComponent<TextMesh>();
                text.fontSize = 12;
            }
            else
            {
                if (touchedByZoomRaycast != null)
                {
                    var text = touchedByZoomRaycast.transform.GetChild(1).GetComponent<TextMesh>();
                    text.fontSize = 2;
                    touchedByZoomRaycast = null;
                }
            }
        }
        else
        {
            if (touchedByZoomRaycast != null)
            {
                var text = touchedByZoomRaycast.transform.GetChild(1).GetComponent<TextMesh>();
                text.fontSize = 2;
                touchedByZoomRaycast = null;
            }
        }

    }

    #region Actions

    void placeHolderUICiblage()
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
                        ModuleManager mm = hit.transform.GetChild(0).GetComponent<ModuleManager>();

                        if (mm.cartesModule.Count >= 2)
                        {
                            if (zoneSelectionADV == null) // Si on vise adversaire
                            {
                                zoneSelectionADV = GameObject.Instantiate(mm.cartesModule[2].prefabZoneSelection);
                                zoneSelectionADV.GetComponent<parent>().CheckOverlap(ennemiManager.sallesRT);
                                var pouett = mm.MyModules[1].transform.rotation;
                                zoneSelectionADV.transform.rotation = pouett;
                            }
                            // else sur nest

                        }

                        if (Mathf.Abs(moletteSouris) > 0)
                        {
                            mm.MyModules[1].transform.Rotate(0, 0, 90 * (moletteSouris * 10));
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

                    //uiPlaceHolder.SetActive(false);
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && uiPlaceHolder.activeInHierarchy)
        {
            //uiPlaceHolder.SetActive(false);
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
                    ModuleManager mm = hit.transform.GetChild(0).GetComponent<ModuleManager>();


                    if (!cartesManager.CheckHandisFull())
                    {
                        mm.MyModules[mm.cartesModule.Count - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
                            Resources.Load<Sprite>("Sprites/Cartes/Picto/cercleBlanc");
                        if (!CartesManager.PhaseLente)
                        {

                            cartesManager.AjouterUneCarteDansLaMain(1, mm.cartesModule[mm.cartesModule.Count - 1].cartesTypes);
                            cartesManager.ModuleToHand(mm);
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
                    ModuleManager mm = hit.transform.GetChild(0).GetComponent<ModuleManager>();
                    caj.GetComponent<Animator>().SetBool("Dance", true);
                    hittingAModule = true;
                    moduleHit = mm.MyModules[mm.cartesModule.Count-1];

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
        var c = moduleHit;
        ModuleManager mm = moduleHit.transform.parent.GetComponent<ModuleManager>();
        cartesManager.PlayACardOnModule(cardIDBeingPlayed, c);
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
        if (wantedName == "Attaque 01") // Pour viser adversaire (il va falloir trouver mieux)
        {
            zoneSelectionADV = GameObject.Instantiate(mm.cartesModule[2].prefabZoneSelection);
            parent overlapScript = zoneSelectionADV.GetComponent<parent>();
            var calculTest = (Mathf.Abs(mm.rotationCompteur) % 4);
            zoneSelectionADV.transform.Rotate(0, 0, 90 * calculTest);
            var sallesTouchees = overlapScript.CheckOverlapADV(ennemiManager.sallesRT);
            Destroy(zoneSelectionADV);
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
        else // Pour viser NEST (il va falloir trouver mieux)
        {

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
