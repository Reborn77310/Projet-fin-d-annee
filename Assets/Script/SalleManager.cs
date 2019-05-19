using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Salles
{

    public GameObject MyGo;
    public bool CanPlayHere = true;
    public float pv;

    public bool isDefendu;
    public int DefendingAmount = 0;

    public bool isReparing = false;
    public bool isAttacked = false;

    public Salles(GameObject go)
    {
        MyGo = go;
        pv = 100;
        isDefendu = false;
    }
}

public class SalleManager : MonoBehaviour
{
    private SalleSound salleSound;
    public List<Salles> allSalles = new List<Salles>();
    public TextMeshProUGUI[] pvSalles; // A ASSIGNER
    public TextMeshProUGUI pvDuVehiculeText;
    public float pvDuVehicule = 300;
    public float pvDuVehiculeMax = 300;
    private SalleManager instance;
    public EnnemiManager ennemiManager;
    public RectTransform[] sallesRT;

    void Start()
    {
        salleSound = GameObject.Find("Salles V4").GetComponent<SalleSound>();
        instance = this;
        InitializeSalles();
        pvDuVehiculeText.text = Mathf.RoundToInt(pvDuVehicule / pvDuVehiculeMax * 100).ToString() + " %";
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

            pvSalles[salleVisee].text = Mathf.RoundToInt(allSalles[salleVisee].pv).ToString() + " %";
            pvDuVehiculeText.text = Mathf.RoundToInt(pvDuVehicule / pvDuVehiculeMax * 100).ToString() + " %";
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
            pvSalles[salleVisee].text = Mathf.RoundToInt(allSalles[salleVisee].pv).ToString() + " %";
            pvDuVehicule -= damage;
            pvDuVehiculeText.text = Mathf.RoundToInt(pvDuVehicule / pvDuVehiculeMax * 100).ToString() + " %";
            if (pvDuVehicule <= 0)
            {
                // pvDuVehiculeText.text = "MISSANDEI ?!?";
                //   Time.timeScale = 0;
            }
            if (allSalles[salleVisee].pv <= 0)
            {
                allSalles[salleVisee].pv = 0;
                if (!allSalles[salleVisee].isReparing)
                {
                    allSalles[salleVisee].CanPlayHere = false;
                    StartCoroutine("ReparationSalle", salleVisee);
                    allSalles[salleVisee].isReparing = true;
                }

            }
        }
    }

    public void MakeCooldownSalle(int SalleVisee, float Cooldown)
    {

        StartCoroutine(instance.CooldownSalle(SalleVisee, Cooldown));
    }

    IEnumerator CooldownSalle(int salleVisee, float cooldown)
    {
        allSalles[salleVisee].CanPlayHere = false;
        yield return new WaitForSeconds(cooldown);
        if (!allSalles[salleVisee].isReparing)
        {
            allSalles[salleVisee].CanPlayHere = true;
        }

        ModuleManager mm = allSalles[salleVisee].MyGo.GetComponent<ModuleManager>();
        mm.slotImage[2].sprite = mm.defaultSprite[2];
        mm.cartesModule.RemoveAt(2);
        mm.MyCompteurInt -= 1;

        if (mm.MyCompteurInt <= 0)
        {
            mm.MyCompteurInt = 2;
            Camera.main.GetComponent<CardSound>().CardBurn();
            mm.cartesModule.RemoveAt(1);
            mm.slotImage[1].sprite = mm.defaultSprite[1];
        }

        yield break;
    }

    IEnumerator ReparationSalle(int salleVisee)
    {
        allSalles[salleVisee].CanPlayHere = false;
        salleSound.DetruireLaSalle();

        while (allSalles[salleVisee].pv < 100)
        {
            allSalles[salleVisee].pv += 5 * Time.deltaTime;
            pvSalles[salleVisee].text = Mathf.RoundToInt(allSalles[salleVisee].pv).ToString() + " %";
            yield return new WaitForSeconds(Time.deltaTime);
        }
        allSalles[salleVisee].CanPlayHere = true;
        allSalles[salleVisee].pv = 100;
        pvSalles[salleVisee].text = Mathf.RoundToInt(allSalles[salleVisee].pv).ToString() + " %";
        salleSound.ReparerLaSalle();
        allSalles[salleVisee].isReparing = false;
        yield break;
    }

    public void DefendreSalle(int salleVisee)
    {
        pvSalles[salleVisee].text = "DEF";
    }

    public void CancelDefense(int salleVisee)
    {
        pvSalles[salleVisee].text = Mathf.RoundToInt(allSalles[salleVisee].pv).ToString() + " %";
    }
}
