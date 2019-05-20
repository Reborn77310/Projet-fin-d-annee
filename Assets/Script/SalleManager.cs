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
    public int etat = 0; // 0 = fonctionnelle, 1 = cooldown, 2 = reparation
    public float timer = 0;
    public float facteurReparation = 1;
    public float facteurCooldown = 1;

    public bool isAttacked = false;

    public Salles(GameObject go)
    {
        MyGo = go;
        pv = 100;
    }
}

public class SalleManager : MonoBehaviour
{
    public class Effets
    {
        public float duration;
        public string name;
        public string[] tags;
        public int[] salles;
        public Effets()
        {

        }
    }
    private SalleSound salleSound;
    public List<Salles> allSalles = new List<Salles>();
    public List<Effets> allEffets = new List<Effets>();
    public TextMeshProUGUI[] pvSalles;
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

    void Update() {
        GestionEtatDesSalles();
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
        allSalles[salleVisee].pv -= damage;
        pvSalles[salleVisee].text = Mathf.RoundToInt(allSalles[salleVisee].pv).ToString() + " %";
        pvDuVehicule -= damage;
        pvDuVehiculeText.text = Mathf.RoundToInt(pvDuVehicule / pvDuVehiculeMax * 100).ToString() + " %";

        if (allSalles[salleVisee].pv <= 0)
        {
            allSalles[salleVisee].pv = 0;
            if (allSalles[salleVisee].etat != 2)
            {
                allSalles[salleVisee].CanPlayHere = false;
                allSalles[salleVisee].etat = 2;
            }
        }
    }

    public void EnterCooldown(int salleVisee, float cooldown)
    {
        allSalles[salleVisee].CanPlayHere = false;
        allSalles[salleVisee].etat = 1;
        allSalles[salleVisee].timer = cooldown;
    }

    public void GestionDurabiliteFinCD(int salleVisee)
    {
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
    }
    public void GestionEtatDesSalles()
    {
        for (int i = 0; i < allSalles.Count; i++)
        {

            if (allSalles[i].etat == 0) // FONCTIONNELLE
            {

            }
            else if (allSalles[i].etat == 1) // COOLDOWN
            {
                allSalles[i].timer -= Time.deltaTime * allSalles[i].facteurCooldown;
                if (allSalles[i].timer <= 0)
                {
                    GestionDurabiliteFinCD(i);
                    allSalles[i].etat = 0;
                    allSalles[i].timer = 0;
                    allSalles[i].CanPlayHere = true;
                }
            }
            else if (allSalles[i].etat == 2) // REPARATION
            {
                if (allSalles[i].timer > 0)
                {
                    GestionDurabiliteFinCD(i);
                    allSalles[i].timer = 0;
                }

                allSalles[i].pv += 5 * Time.deltaTime * allSalles[i].facteurReparation;
                pvSalles[i].text = Mathf.RoundToInt(allSalles[i].pv).ToString() + " %";
                if (allSalles[i].pv >= 100)
                {
                    allSalles[i].timer = 0;
                    allSalles[i].CanPlayHere = true;
                    allSalles[i].pv = 100;
                    pvSalles[i].text = Mathf.RoundToInt(allSalles[i].pv).ToString() + " %";
                    salleSound.ReparerLaSalle();
                    allSalles[i].etat = 0;
                }
            }

        }
    }
}
