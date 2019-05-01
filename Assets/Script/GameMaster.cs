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

    void Awake()
    {
        cardSound = Camera.main.GetComponent<CardSound>();
        cartesManager = GetComponent<CartesManager>();
        cajSR = caj.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
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
            if (hit.transform.tag == "Module")
            {
                ModuleManager MM = hit.transform.parent.GetComponent<ModuleManager>();
                for (int i = 0; i < MM.MyModules.Length; i++)
                {
                    if (hit.transform.gameObject == MM.MyModules[i].MyObject && MM.MyModules[i].MyType != Modules.Type.Droite)
                    {
                        if (!cartesManager.CheckHandisFull() && MM.MyModules[i].CarteType >= 0)
                        {
                            if (!CartesManager.PhaseLente)
                            {
                                cartesManager.AjouterUneCarteDansLaMain(1,MM.MyModules[i].CarteType);
                            }
                            
                            MM.MyModules[i].MyObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
                                Resources.Load<Sprite>("Sprites/Cartes/Picto/cercleBlanc");
                            MM.MyModules[i].CarteType = -1;
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
            if (hit.transform.tag == "Module")
            {
                ModuleManager mm = hit.transform.parent.GetComponent<ModuleManager>();
                for (int i = 0; i < mm.MyModules.Length; i++)
                {
                    if (hit.transform.gameObject == mm.MyModules[i].MyObject)
                    {
                        caj.GetComponent<Animator>().SetBool("Dance", true);
                        hittingAModule = true;
                        moduleHit = hit.transform.gameObject;
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
