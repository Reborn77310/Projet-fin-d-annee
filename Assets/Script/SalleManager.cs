using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Salles
{
    public GameObject SpawnParticleFeedback;
    public GameObject ActualFeedbackOnMe;
    public GameObject MyGo;
    public bool CanPlayHere = true;
    public float pv;
    public int etat = 0; // 0 = fonctionnelle, 1 = cooldown, 2 = reparation
    public float timer = 0;
    public float timerMax = 0;
    public float facteurReparation = 1;
    public float facteurCooldown = 1;
    public bool canBeTarget = true;
    public bool canOverdrive = true;
    public bool isAttacked = false;

    public Salles(GameObject go)
    {
        MyGo = go;
        pv = 100;
        SpawnParticleFeedback = MyGo.transform.GetChild(0).gameObject;
    }
}

public class SalleManager : MonoBehaviour
{
    public class Effets
    {
        public float duration;
        public string name;
        public string[] tags;
        public int salle;
        public float value;
        public GameObject Feedback;
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
    public Animator[] animators;
    public Image[] fillCD;

    void Start()
    {
        salleSound = GameObject.Find("Salles V4").GetComponent<SalleSound>();
        instance = this;
        InitializeSalles();
        pvDuVehiculeText.text = Mathf.RoundToInt(pvDuVehicule / pvDuVehiculeMax * 100).ToString() + " %";
    }

    void Update()
    {
        GestionEtatDesSalles();
        GestionDesEffets();
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

    public void DamageSurSalle(int salleVisee, float damage)
    {
        allSalles[salleVisee].pv -= damage;
        pvSalles[salleVisee].text = Mathf.RoundToInt(allSalles[salleVisee].pv).ToString() + " %";
        pvDuVehicule -= damage;
        pvDuVehiculeText.text = Mathf.RoundToInt(pvDuVehicule / pvDuVehiculeMax * 100).ToString() + " %";
        animators[salleVisee].SetTrigger("hit");
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
        allSalles[salleVisee].timerMax = cooldown;
        fillCD[salleVisee].fillAmount = 1;
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
                fillCD[i].fillAmount = allSalles[i].timer / allSalles[i].timerMax;
                if (allSalles[i].timer <= 0)
                {
                    fillCD[i].fillAmount = 0;
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
                    // if(animators[i].GetBool("cooldown"))
                    // {
                    //     animators[i].SetBool("cooldown", false);
                    // }
                    fillCD[i].fillAmount = 0;
                    GestionDurabiliteFinCD(i);
                    allSalles[i].timer = 0;
                }

                allSalles[i].pv += 5 * Time.deltaTime * allSalles[i].facteurReparation;
                pvSalles[i].text ="<color=#4061BA>" + Mathf.RoundToInt(allSalles[i].pv).ToString() + " % </color>";
                if (allSalles[i].pv >= 100)
                {
                    pvSalles[i].text = "<color=#78FF1B>" + Mathf.RoundToInt(allSalles[i].pv).ToString() + " % </color>"; 
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

    public void GestionDesEffets()
    {
        for (int i = 0; i < allEffets.Count; i++)
        {
            allEffets[i].duration -= Time.deltaTime;
            if (allEffets[i].duration <= 0)
            {
                RemoveEffets(i);
                allEffets.RemoveAt(i);
            }
        }
    }

    public void RemoveEffets(int i)
    {
        if (allEffets[i].name == "Tempo")
        {
            allSalles[allEffets[i].salle].facteurCooldown -= allEffets[i].value;
        }
        else if (allEffets[i].name == "Brouilleur")
        {
            allSalles[allEffets[i].salle].canBeTarget = true;
        }
        else if (allEffets[i].name == "Singularity")
        {
            allSalles[allEffets[i].salle].canOverdrive = true;
            GameObject.Find("Salle_NEST_"+ allEffets[i].salle).transform.GetChild(0).GetComponent<Animator>().SetBool("rouge", false);
        }
        else if (allEffets[i].name == "Grappin")
        {
        ennemiManager.brouillage.SetActive(false);
        }
        else if (allEffets[i].name == "Smoke")
        {
            Destroy(allEffets[i].Feedback);
        }
    }

    public void AddEffets(float _duration, string _name, string[] _tags, int _salle, float _value)
    {
        Effets a = new Effets();

        a.duration = _duration;
        a.name = _name;
        a.tags = _tags;
        a.salle = _salle;
        a.value = _value;
        
        allEffets.Add(a);
    }
}
