using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SalleScript : MonoBehaviour
{

    public Material normalMaterial;
    public Material onCardDrag;
    public MeshRenderer mr;
    public int id;
    public SpriteRenderer sr;
    public SpriteRenderer textConsole;
    public GameObject myModule;
    public bool cardHasBeenPlayedHere = false;

    void Awake()
    {
    }

    public void OnDragCardOnMe()
    {
        // mr.material = normalMaterial;
        // if(mr.gameObject.transform.childCount > 0)
        // {
        // 	mr.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = normalMaterial;
        // }
    }

    public void OnExitCardOnMe()
    {
        // mr.material = onCardDrag;
        // if(mr.gameObject.transform.childCount > 0)
        // {
        // 	mr.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = onCardDrag;
        // }
    }

    public void ChangeForText()
    {
        sr.sprite = null;
        textConsole.enabled = true;
    }

    public void CheckMaterial()
    {
        if (mr.material.name.Contains(normalMaterial.name))
        {
            if (GameMaster.moduleHit == null)
            {
                OnExitCardOnMe();
            }
            else
            {
                if (GameMaster.moduleHit != myModule)
                {
                    OnExitCardOnMe();
                }
            }
        }
    }
}
