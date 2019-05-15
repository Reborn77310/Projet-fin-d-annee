using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Salles
{

    public GameObject MyGo;
    public bool CanPlayHere = true;
    public int pv;

    public bool isDefendu;
    public int DefendingAmount = 0;

    public bool isAttacked = false;
    public string[] equipement = new string[2];
    public bool[] equipementATK = new bool[2];

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
    public TextMeshProUGUI[] pvSalles; // A ASSIGNER
    public TextMesh[] SalleOnCooldown = new TextMesh[4]; // A ASSIGNER
    public TextMeshProUGUI pvDuVehiculeText;
    public int pvDuVehicule = 300;
    public int pvDuVehiculeMax = 300;
    private SalleManager instance;
    public EnnemiManager ennemiManager;
    public RectTransform[] sallesRT;

    void Start()
    {
        instance = this;
        InitializeSalles();
        pvDuVehiculeText.text = (pvDuVehicule / pvDuVehiculeMax * 100).ToString() + " %";
        allSalles[0].equipement[0] = "Cape d'invisibilité";
        allSalles[1].equipement[0] = "Cape d'invisibilité";
        allSalles[2].equipement[0] = "Attaque 03";
        allSalles[3].equipement[0] = "Attaque 03";
        allSalles[0].equipementATK[0] = false;
        allSalles[1].equipementATK[0] = false;
        allSalles[2].equipementATK[0] = true;
        allSalles[3].equipementATK[0] = true;
    }

    public void ChangeMaterial()
    {
        for (int i = 0; i < allSalles.Count; i++)
        {
            if (allSalles[i].isAttacked)
            {

            }
            else
            {

            }
        }
    }

    void InitializeSalles()
    {
        for (int i = 0; i < 4; i++)
        {
            Salles newSalle = new Salles(GameObject.Find("Salle" + i.ToString()));
            allSalles.Add(newSalle);
            //pvSalles[i].GetComponent<TextMesh>().text = allSalles[i].pv.ToString();
        }
    }

    public void DamageSurSalle(int salleVisee, int damage)
    {
        if (allSalles[salleVisee].isDefendu)
        {
            allSalles[salleVisee].isDefendu = false;
            if (allSalles[salleVisee].DefendingAmount < damage)
            {
                var wantedDamage = damage - allSalles[salleVisee].DefendingAmount;
                allSalles[salleVisee].pv -= wantedDamage;
                pvDuVehicule -= wantedDamage;
            }

            pvSalles[salleVisee].text = allSalles[salleVisee].pv.ToString() + " %";
            pvDuVehiculeText.text = (pvDuVehicule / pvDuVehiculeMax * 100).ToString() + " %";
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

            allSalles[salleVisee].DefendingAmount = 0;
        }
        else
        {
            allSalles[salleVisee].pv -= damage;
            pvSalles[salleVisee].text = allSalles[salleVisee].pv.ToString() + " %";
            pvDuVehicule -= damage;
            pvDuVehiculeText.text = (pvDuVehicule / pvDuVehiculeMax * 100).ToString() + " %";
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

    public void MakeCooldownSalle(int SalleVisee, float Cooldown)
    {
        allSalles[SalleVisee].MyGo.GetComponent<ModuleManager>().MyModules[0].transform.parent.transform.GetChild(2).GetComponent<Image>().color = Color.red;
        StartCoroutine(instance.CooldownSalle(SalleVisee, Cooldown));
    }

    IEnumerator CooldownSalle(int salleVisee, float cooldown)
    {
        allSalles[salleVisee].CanPlayHere = false;
        SalleOnCooldown[salleVisee].text = "YES";
        yield return new WaitForSeconds(cooldown);
        if (allSalles[salleVisee].pv > 0)
        {
            allSalles[salleVisee].CanPlayHere = true;
            SalleOnCooldown[salleVisee].text = "NO";
            allSalles[salleVisee].MyGo.GetComponent<ModuleManager>().MyModules[0].transform.parent.transform.GetChild(2).GetComponent<Image>().color = Color.white;
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
        allSalles[salleVisee].MyGo.GetComponent<ModuleManager>().MyModules[0].transform.parent.transform.GetChild(2).GetComponent<Image>().color = Color.white;
        yield break;
    }

    public void DefendreSalle(int salleVisee)
    {
        pvSalles[salleVisee].text = "DEF";
    }

    public void CancelDefense(int salleVisee)
    {
        pvSalles[salleVisee].text = allSalles[salleVisee].pv.ToString() + " %";
    }
}
