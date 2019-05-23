using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionBoutonUi : MonoBehaviour
{
    public Button Almanac;
    public Button Equipement;
    public Button Suivant;
    public Button Precedent;
    public Button Retour;
    public GameObject Canvas2;
    public GameObject HorsCombat;
    public void ClickAlmanac()
    {
        if (Suivant.gameObject.activeInHierarchy)
        {
            Suivant.gameObject.SetActive(false);
            Precedent.gameObject.SetActive(false);
            Retour.gameObject.SetActive(false);
        }
        else
        {
            Suivant.gameObject.SetActive(true);
            Precedent.gameObject.SetActive(true);
            Retour.gameObject.SetActive(true);
        }
        Camera.main.GetComponent<Animator>().SetTrigger("Zoom");
        Canvas2.SetActive(true);
        Canvas2.transform.GetChild(0).gameObject.SetActive(true);
        HorsCombat.SetActive(false);
    }

    public void ClickEquipement()
    {
        if (Suivant.gameObject.activeInHierarchy)
        {
            Suivant.gameObject.SetActive(false);
            Precedent.gameObject.SetActive(false);
            Retour.gameObject.SetActive(false);
        }
        else
        {
            Suivant.gameObject.SetActive(true);
            Precedent.gameObject.SetActive(true);
            Retour.gameObject.SetActive(true);
        }
        Camera.main.GetComponent<Animator>().SetTrigger("Zoom");
        Canvas2.SetActive(true);
        Canvas2.transform.GetChild(1).gameObject.SetActive(true);
        HorsCombat.SetActive(false);
    }
}
