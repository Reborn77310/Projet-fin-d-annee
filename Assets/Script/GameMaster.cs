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

    private CardSound cardSound;
    private GameObject touchedByZoomRaycast;
    
    void Awake()
    {
        cardSound = Camera.main.GetComponent<CardSound>();
        cartesManager = GetComponent<CartesManager>();
        cajSR = caj.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var moletteSouris = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(moletteSouris) > 0)
        {
            RotateSecondModule(moletteSouris);
        }
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

    public void RotateSecondModule(float valeur)
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
                ModuleManager mm = hit.transform.GetChild(0).GetComponent<ModuleManager>();
                mm.MyModules[1].MyObject.transform.Rotate(0,0,90 * (valeur * 10));
                mm.MyModules[1].rotationCompteur += 1 * (Mathf.RoundToInt(valeur * 10));
            }
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
                ModuleManager mm = hit.transform.GetChild(0).GetComponent<ModuleManager>();
                var testBool = false;
                for (int i = mm.MyModules.Length - 1; i >= 0; i--)
                {
                    if (mm.MyModules[i].CarteType != -1 && !testBool && mm.MyModules[i].MyType != Modules.Type.Droite)
                    {
                        if (!cartesManager.CheckHandisFull())
                        {
                            testBool = true;
                            if (!CartesManager.PhaseLente)
                            {
                                cartesManager.AjouterUneCarteDansLaMain(1,mm.MyModules[i].CarteType);
                            }
                            
                            mm.MyModules[i].MyObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
                                Resources.Load<Sprite>("Sprites/Cartes/Picto/cercleBlanc");
                            mm.MyModules[i].CarteType = -1;
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
                ModuleManager mm = hit.transform.GetChild(0).GetComponent<ModuleManager>();
                var testBool = false;
                for (int i = 0; i < mm.MyModules.Length; i++)
                {
                    if (mm.MyModules[i].CarteType == -1 && !testBool)
                    {
                        testBool = true;
                        caj.GetComponent<Animator>().SetBool("Dance", true);
                        hittingAModule = true;
                        moduleHit = mm.MyModules[i].MyObject;
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

    public void PlayCard()
    {
        var c = moduleHit;
        ModuleManager mm = moduleHit.transform.parent.GetComponent<ModuleManager>();
        
        if (mm.MyModules[0].CarteType != -1 && mm.MyModules[1].CarteType != -1 && mm.MyModules[2].MyObject == moduleHit && !CartesManager.PhaseLente)
        {
            cartesManager.PlayACardOnModule(cardIDBeingPlayed, c);
            cardSound.GoingToPlayACard();     
            cardIDBeingPlayed = -1;
        }
        else if (mm.MyModules[2].MyObject != moduleHit)
        {
            cartesManager.PlayACardOnModule(cardIDBeingPlayed, c);
            cardSound.GoingToPlayACard();     
            cardIDBeingPlayed = -1;
        }
    }
    #endregion

}
