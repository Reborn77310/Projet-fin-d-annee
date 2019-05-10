using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Salles
{
    public GameObject MyGo;
    public bool CanPlayHere = true;
    public int pv;

    public bool isDefendu;

    public Salles(GameObject go)
    {
        MyGo = go;
        pv = 100;
        isDefendu = false;
    }
}

public class SalleManager : MonoBehaviour
{

    public List<Salles> allSalles = new List<Salles>();
    public GameObject[] pvSalles;
    public TextMesh[] SalleOnCooldown = new TextMesh[4];
    public TextMesh pvDuVehiculeText;
    public int pvDuVehicule = 300;
    private SalleManager instance;
    
    void Start()
    {
        instance = this;
        InitializeSalles();
        pvDuVehiculeText.text = pvDuVehicule.ToString();
    }

    void InitializeSalles()
    {
        for (int i = 0; i < 4; i++)
        {
            Salles newSalle = new Salles(GameObject.Find("Salle" + i.ToString()));
            allSalles.Add(newSalle);
            pvSalles[i].GetComponent<TextMesh>().text = allSalles[i].pv.ToString();
            SalleOnCooldown[i] = allSalles[i].MyGo.transform.GetChild(12).GetComponent<TextMesh>();
        }
    }

    public void DamageSurSalle(int salleVisee, int damage)
    {
        if (allSalles[salleVisee].isDefendu)
        {
            allSalles[salleVisee].isDefendu = false;
        }
        else
        {
            allSalles[salleVisee].pv -= damage;
            pvSalles[salleVisee].GetComponent<TextMesh>().text = allSalles[salleVisee].pv.ToString();
            pvDuVehicule -= damage;
            pvDuVehiculeText.text = pvDuVehicule.ToString();
            if (pvDuVehicule <= 0)
            {
                pvDuVehiculeText.text = "MISSANDEI ?!?";
                Time.timeScale = 0;
            }
            if (allSalles[salleVisee].pv <= 0)
            {
                allSalles[salleVisee].pv = 0;
                allSalles[salleVisee].CanPlayHere = false;
                ReparationSalle(salleVisee);
            }
        }
    }

    public void MakeCooldownSalle(int SalleVisee,int Cooldown)
    {
        StartCoroutine(instance.CooldownSalle(SalleVisee,Cooldown));
    }

    IEnumerator CooldownSalle(int salleVisee, int cooldown)
    {
        allSalles[salleVisee].CanPlayHere = false;
        SalleOnCooldown[salleVisee].text = "YES";
        yield return new WaitForSeconds(cooldown);
        if (allSalles[salleVisee].pv > 0)
        {
            allSalles[salleVisee].CanPlayHere = true;
            SalleOnCooldown[salleVisee].text = "NO";
        }
        yield break;
    }
    
    IEnumerator ReparationSalle(int salleVisee)
    {
        SalleOnCooldown[salleVisee].text = "YES";
        yield return new WaitForSeconds(30);
        allSalles[salleVisee].CanPlayHere = true;
        allSalles[salleVisee].pv = 100;
        SalleOnCooldown[salleVisee].text = "NO";
        yield break;
    }

    public void DefendreSalle(int salleVisee)
    {
        pvSalles[salleVisee].GetComponent<TextMesh>().text = "You shall not pass !";
    }

    public void CancelDefense(int salleVisee)
    {
        pvSalles[salleVisee].GetComponent<TextMesh>().text =  allSalles[salleVisee].pv.ToString();
    }
}
