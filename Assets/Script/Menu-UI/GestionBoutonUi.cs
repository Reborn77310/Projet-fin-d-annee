using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class  GestionBoutonUi : MonoBehaviour
{
    public Button Almanac;
    public Button Equipement;

    public GameObject Canvas2;
    public GameObject HorsCombat;
    public GameObject fondu1;
    public GameObject fondu2;

    public int ActualRoom = 0;
    
    GestionEquipement ge;
    GameMaster gm;
    
    void Awake()
    {
        ge = GetComponent<GestionEquipement>();
        gm = GetComponent<GameMaster>();
    }
    
    public void ClickAlmanac(GameObject go)
    {
        Color a = Color.white;
        a.a = 0;
        go.GetComponent<Image>().color = a;
        
        Camera.main.GetComponent<Animator>().SetTrigger("Zoom");
        if (!Canvas2.activeInHierarchy)
        {
            Canvas2.SetActive(true);
        }
        if (HorsCombat.activeInHierarchy)
        {
            HorsCombat.SetActive(false);
        }
        Canvas2.transform.GetChild(0).gameObject.SetActive(true);
        Canvas2.transform.GetChild(1).gameObject.SetActive(false);
        Camera.main.GetComponent<CanvaSound>().LancerAlmanach();
    }
    public void ClickEquipement()
    {
        Camera.main.GetComponent<Animator>().SetTrigger("Zoom");
        if (!Canvas2.activeInHierarchy)
        {
            Canvas2.SetActive(true);
        }
        if (HorsCombat.activeInHierarchy)
        {
            HorsCombat.SetActive(false);
        }
        Canvas2.transform.GetChild(0).gameObject.SetActive(false);
        Canvas2.transform.GetChild(1).gameObject.SetActive(true);
        Camera.main.GetComponent<CanvaSound>().LancerEquipement();
    }
    

    public void ChangeToEquip()
    {
        StartCoroutine("FonduAuNoirAlmaToEquip");
        Camera.main.GetComponent<CanvaSound>().LancerEquipement();
    }
    void Change1()
    {
        Canvas2.transform.GetChild(0).gameObject.SetActive(false);
        Canvas2.transform.GetChild(1).gameObject.SetActive(true);
        Color col = fondu2.GetComponent<Image>().color;
        col.a = 1;
        fondu2.GetComponent<Image>().color = col;
    }
    IEnumerator FonduAuNoirAlmaToEquip()
    {
        while (fondu1.GetComponent<Image>().color.a < 1)
        {
            Color col = fondu1.GetComponent<Image>().color;
            col.a += Time.deltaTime;
            fondu1.GetComponent<Image>().color = col;

            yield return new WaitForSeconds(Time.deltaTime);
        }
        Change1();
        while (fondu2.GetComponent<Image>().color.a > 0)
        {
            Color col = fondu2.GetComponent<Image>().color;
            col.a -= Time.deltaTime;
            fondu2.GetComponent<Image>().color = col;
            yield return new WaitForSeconds(Time.deltaTime * 2);
        }
    }
    
    public void ChangeToAlma()
    {
        StartCoroutine("FonduAuNoirEquipToAlma");
        Camera.main.GetComponent<CanvaSound>().LancerEquipement();
    }
    void Change2()
    {
        Canvas2.transform.GetChild(0).gameObject.SetActive(true);
        Canvas2.transform.GetChild(1).gameObject.SetActive(false);
        Color col = fondu1.GetComponent<Image>().color;
        col.a = 1;
        fondu1.GetComponent<Image>().color = col;
    }
    IEnumerator FonduAuNoirEquipToAlma()
    {
        while (fondu2.GetComponent<Image>().color.a < 1)
        {
            Color col = fondu2.GetComponent<Image>().color;
            col.a += Time.deltaTime;
            fondu2.GetComponent<Image>().color = col;

            yield return new WaitForSeconds(Time.deltaTime);
        }
        Change2();
        while (fondu1.GetComponent<Image>().color.a > 0)
        {
            Color col = fondu1.GetComponent<Image>().color;
            col.a -= Time.deltaTime;
            fondu1.GetComponent<Image>().color = col;
            yield return new WaitForSeconds(Time.deltaTime * 2);
        }
    }

    public void RetourVersVehicule()
    {
        Camera.main.GetComponent<CanvaSound>().LancerFermeture();
        HorsCombat.SetActive(true);
        Camera.main.GetComponent<Animator>().speed = 0.6f;
        Camera.main.GetComponent<Animator>().SetTrigger("GoUp");
        Canvas2.transform.GetChild(0).gameObject.SetActive(false);
        Canvas2.transform.GetChild(1).gameObject.SetActive(false);
        gm.RetourAlmanac();
    }
    
    public void ChangeRoom(GameObject ButtonClicked)
    {
        int ButtonNumber = int.Parse(ButtonClicked.name);
        if (ActualRoom != ButtonNumber)
        {
            ge.ChangeRoom(ButtonNumber);
        }
    }
}
