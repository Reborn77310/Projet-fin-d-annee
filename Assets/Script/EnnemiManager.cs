using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class EnnemiRooms
{
    public RectTransform ui;
    public Sprite normale;
    public Sprite hightlight;
    public int[] myType;
    public Image[] symbole;
    public float pv = 100;
    public float pvMax = 100;
    public TextMeshPro pvText;
    public bool isDead = false;
    public float timer;
    public int etat = 1;
    public int salleFocus;
    public bool isAttacking = false;
    public EnnemiRooms(float _timer)
    {
        timer = _timer;
    }
}

public class EnnemiManager : MonoBehaviour
{
    public List<EnnemiRooms> ennemiRooms = new List<EnnemiRooms>();
    public SalleManager salleManager;
    public float pvTotaux;
    public float pvTotauxMax;
    public GameObject badGuy = null;
    public Canvas myCanvas;
    CartesManager cartesManager;
    private AllSetupsActions allSetup;

    public GameObject horsCombat; // Les éléments d'ui hors combat
    public GameObject combat; // Les éléments d'ui en combat

    public GameObject prefabADV;
    public GameObject boutonSpawn;

    public Image[] BDM_Symbole;
    public TextMeshProUGUI[] DBM_Cible;
    public TextMeshProUGUI[] DBM_timer;

    public TextMeshProUGUI pvTotauxText;
    public TextMeshProUGUI infosNext;

    public struct DBM
    {
        public int origine; // index de la salle d'origine dans ennemiRooms
        public int cible; // index de la cible
        public int timer; // Temps restant avant lancement de l'action
        public int id; //Correspond à l'id de l'action qui servira à appeler la fonction correspondante.
                       // Si l'ID est pair la cible se trouve sur l'ADV sinon c'est sur NEST
    }

    public List<DBM> actionPrevues = new List<DBM>();

    private void Awake()
    {
        allSetup = GetComponent<AllSetupsActions>();
        cartesManager = GetComponent<CartesManager>();
        salleManager = GetComponent<SalleManager>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }


    void Update()
    {
        if (badGuy != null)
        {
            CheckPdv();
            //GestionDesSalles();
            MajDBM();
        }

    }

    public void RecupInfosADV(RectTransform _ui, Sprite _normale, Sprite _HL, float _pv, GameObject _text)
    {
        EnnemiRooms a = new EnnemiRooms(ennemiRooms.Count * 3 + Random.Range(1, 5));
        a.ui = _ui;
        a.normale = _normale;
        a.hightlight = _HL;
        a.pvMax = _pv;
        a.pv = _pv;
        a.pvText = _text.GetComponent<TextMeshPro>();
        ennemiRooms.Add(a);
    }

    public void RecupFormule(int[] _nb, int[] _types, Image[] _symboles)
    {
        int index = 0;
        for (int i = 0; i < ennemiRooms.Count; i++)
        {
            for (int h = 0; h < _nb[i]; h++)
            {
                ennemiRooms[i].myType[h] = _types[index];
                ennemiRooms[i].symbole[h] = _symboles[index];
                index++;
            }
        }
    }

    // public void SpawnMob(int nbSalles)
    // {
    //     cartesManager.ennemiManager = this;
    //     allSetup.ennemiManager = this;
    //     salleManager.ennemiManager = this;

    //     badGuy = GameObject.Instantiate(badGuyPrefab, myCanvas.transform, false);
    //     badGuy.GetComponent<RectTransform>().position = ParentOfBadGuy.position;

    //     intentionMechantes = badGuy.transform.GetChild(1).GetComponentsInChildren<Text>();

    //     for (int i = 0; i < nbSalles; i++)
    //     {
    //         intentionMechantes[i].text = "";
    //         EnnemiRooms a = new EnnemiRooms((Random.Range(1,4)+i*3));
    //         int randomRange = Random.Range(0, 3);
    //         a.myType = randomRange;
    //         a.pictoFormule = badGuy.transform.GetChild(3).GetChild(i).GetComponent<Image>();
    //         a.pictoFormule.sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/Picto" + randomRange);
    //         a.myImagePv = badGuy.transform.GetChild(0).GetChild(i).GetComponent<Image>();
    //         ennemiRooms.Add(a);
    //     }
    //     pv = 300;
    //     cartesManager.DrawCards();
    // }

    public void PerdrePvLocal(int wantedRoom, int wantedDamage)
    {
        ennemiRooms[wantedRoom].pv -= wantedDamage;
        float fill = (ennemiRooms[wantedRoom].pv / ennemiRooms[wantedRoom].pvMax) * 100;
        ennemiRooms[wantedRoom].pvText.text = Mathf.RoundToInt(fill).ToString() + " %";

        if (ennemiRooms[wantedRoom].pv <= 0)
        {
            ennemiRooms[wantedRoom].isDead = true;
            //intentionMechantes[wantedRoom].text = "";
            ennemiRooms[wantedRoom].timer = 20;
            ennemiRooms[wantedRoom].etat = 0;
        }
    }

    public void PerdrePvGlobal(int numberOfPv)
    {
        pvTotaux -= numberOfPv;
        float fill = pvTotaux / pvTotauxMax * 100;
        pvTotauxText.text = Mathf.RoundToInt(fill).ToString() + " %";
    }


    public void CheckPdv()
    {
        if (pvTotaux <= 0)
        {
            Destroy(badGuy);
            Debug.Log("Dracarys");
            boutonSpawn.SetActive(true);
        }
    }

    void GestionDesSalles()
    {
        for (int i = 0; i < ennemiRooms.Count; i++)
        {
            // dead =0
            // elle font rien = 1
            // elle preparent une attack = 2

            ennemiRooms[i].timer -= Time.deltaTime;

            if (ennemiRooms[i].etat == 0)
            {
                ennemiRooms[i].isAttacking = false;
                ennemiRooms[i].pv += 5 * Time.deltaTime;


                if (ennemiRooms[i].pv >= ennemiRooms[i].pvMax)
                {
                    // salle réparée
                    ennemiRooms[i].pv = ennemiRooms[i].pvMax;
                    ennemiRooms[i].isDead = false;
                    ennemiRooms[i].pv = 100;
                    ennemiRooms[i].etat = 1;
                    // intentionMechantes[i].text = "";
                }
            }
            else if (ennemiRooms[i].etat == 1)
            {
                if (ennemiRooms[i].timer <= 0)
                {
                    // Choosenextattaque
                    bool search = true;
                    while (search)
                    {
                        int rnd = Random.Range(0, 4);
                        if (salleManager.allSalles[rnd].pv > 0)
                        {
                            ennemiRooms[i].salleFocus = rnd;
                            ennemiRooms[i].timer = Random.Range(10, 20);
                            search = false;
                        }
                    }
                    ennemiRooms[i].etat = 2;

                }

            }
            else if (ennemiRooms[i].etat == 2)
            {

                ennemiRooms[i].isAttacking = true;
                // intentionMechantes[i].text = "Next Attack" + "\n" + Mathf.RoundToInt(ennemiRooms[i].timer) + "\n" + "Salle " + ennemiRooms[i].salleFocus.ToString();
                // intentionMechantes[i].color = Color.red;
                if (ennemiRooms[i].timer <= 0)
                {
                    // attaque lancée
                    salleManager.DamageSurSalle(ennemiRooms[i].salleFocus, 50);
                    //Play son quand touché
                    // intentionMechantes[i].text = "";
                    ennemiRooms[i].isAttacking = false;
                    // cooldown
                    ennemiRooms[i].timer = Random.Range(8, 16);
                    ennemiRooms[i].etat = 1;
                }
            }
        }
        salleManager.CheckRoomIfAttacked();
    }

    public int[] GiveInfosForDraw()
    {
        int nbSymbolesAlive = 0;
        for (int i = 0; i < ennemiRooms.Count; i++)
        {
            if (!ennemiRooms[i].isDead)
            {
                nbSymbolesAlive += ennemiRooms[i].myType.Length;
            }
        }

        int[] typesToDraw = new int[nbSymbolesAlive];
        int j = 0;
        for (int h = 0; h < ennemiRooms.Count; h++)
        {
            if (!ennemiRooms[h].isDead)
            {
                for (int k = 0; k < ennemiRooms[h].myType.Length; k++)
                {
                    typesToDraw[j] = ennemiRooms[h].myType[k];
                    j++;
                }
            }
        }
        return typesToDraw;
    }

    public void PassageEnPhaseCombat()
    {
        horsCombat.SetActive(false);
        combat.SetActive(true);
        boutonSpawn.SetActive(true);
    }

    public void PassageEnPhaseLente()
    {
        horsCombat.SetActive(true);
        combat.SetActive(false);
        boutonSpawn.SetActive(false);
        if (badGuy != null)
        {
            Destroy(badGuy);
        }
    }

    public void SpawnAdversaire()
    {
        badGuy = GameObject.Instantiate(prefabADV);
        boutonSpawn.SetActive(false);
    }

    public void ChooseAction(int _origine)
    {
        DBM a = new DBM();
        a.timer = Random.Range(10, 20);
        a.origine = _origine;
        a.cible = Random.Range(0, 4);
        a.id = Random.Range(0, 2);

        if (actionPrevues.Count == 0)
        {
            actionPrevues.Add(a);
        }
        else
        {
            for (int i = 0; i < actionPrevues.Count; i++)
            {
                if (a.timer < actionPrevues[i].timer)
                {
                    actionPrevues.Insert(i, a);
                }
            }
            if (!actionPrevues.Contains(a))
            {
                actionPrevues.Add(a);
            }
        }
    }

    public void MajDBM()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < actionPrevues.Count)
            {
                DBM_Cible[i].text = "Room " + actionPrevues[i].cible.ToString();
                DBM_timer[i].text = actionPrevues[i].timer.ToString("F3") + "s";
            }
            else
            {
                DBM_Cible[i].text = "";
                DBM_timer[i].text = "";
            }

        }
    }


}
