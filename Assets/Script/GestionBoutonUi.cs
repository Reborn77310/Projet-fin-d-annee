using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestionBoutonUi : MonoBehaviour
{
    public Button Almanac;
    public Button Equipement;

    public GameObject Canvas2;
    public GameObject HorsCombat;

    public GameObject fondu1;
    public GameObject fondu2;
    public void ClickAlmanac()
    {       
        Camera.main.GetComponent<Animator>().SetTrigger("Zoom");
        Canvas2.SetActive(true);
        GameObject.Find("GameMaster").GetComponent<Almanach>().AfficherInventaire();

        Canvas2.transform.GetChild(0).gameObject.SetActive(true);
        HorsCombat.SetActive(false);

        Camera.main.GetComponent<SonsCanva>().LancerAlmanach();
    }

    public void ChangeToEquip()
    {
        StartCoroutine("changescene");
        Camera.main.GetComponent<SonsCanva>().LancerEquipement();
    }
    void RealChange()
    {
        Canvas2.transform.GetChild(0).gameObject.SetActive(false);
        Canvas2.transform.GetChild(1).gameObject.SetActive(true);
    }
    IEnumerator changescene()
    {
        while (fondu1.GetComponent<Image>().color.a < 1)
        {
            Color col = fondu1.GetComponent<Image>().color;
            col.a += Time.deltaTime;
            fondu1.GetComponent<Image>().color = col;

            yield return new WaitForSeconds(Time.deltaTime);
        }
        RealChange();
        while (fondu2.GetComponent<Image>().color.a > 0)
        {
            Color col = fondu2.GetComponent<Image>().color;
            col.a -= Time.deltaTime;
            fondu2.GetComponent<Image>().color = col;
            yield return new WaitForSeconds(Time.deltaTime * 2);
        }
    }
}
