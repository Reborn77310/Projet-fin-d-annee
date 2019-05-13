using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class DBM
{
    public int origine; // index de la salle d'origine dans ennemiRooms
    public int cible; // index de la cible
    public float timer = 50000; // Temps restant avant lancement de l'action
    public int id = -1; //Correspond à l'id de l'action qui servira à appeler la fonction correspondante.
                        // Si l'ID est pair la cible se trouve sur l'ADV sinon c'est sur NEST
    public DBM()
    {

    }
}

public class EnnemiRooms
{
    public RectTransform ui;
    public Sprite normale;
    public Sprite hightlight;
    public int[] myType;
    public Image[] symbole;
    public float pv = 100;
    public float pvMax = 100;
    public TextMeshProUGUI pvText;
    public bool isDead = false;
    public float timer;
    public int etat = 1;
    public bool isAttacking = false;
    public EnnemiRooms(float _timer)
    {
        timer = _timer;
    }
}

public class EnnemiManager : MonoBehaviour
{
    public List<EnnemiRooms> ennemiRooms = new List<EnnemiRooms>();
    SalleManager salleManager;
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

    public Image[] DBM_Symbole;
    public TextMeshProUGUI[] DBM_Cible;
    public TextMeshProUGUI[] DBM_timer;

    public TextMeshProUGUI pvTotauxText;
    public TextMeshProUGUI infosNext;

    public List<DBM> actionPrevues = new List<DBM>();
    public RectTransform[] sallesRT;

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
            GestionDesActions();
            GestionDesSalles();
            MajDBM();
        }

    }

    public void RecupInfosADV(RectTransform _ui, Sprite _normale, Sprite _HL, float _pv, TextMeshProUGUI _text)
    {
        EnnemiRooms a = new EnnemiRooms(ennemiRooms.Count * 3 + Random.Range(1, 5));
        a.ui = _ui;
        a.normale = _normale;
        a.hightlight = _HL;
        a.pvMax = _pv;
        a.pv = _pv;
        a.pvText = _text;
        ennemiRooms.Add(a);
    }

    public void RecupFormule(int[] _nb, int[] _types, Image[] _symboles)
    {
        int index = 0;
        sallesRT = new RectTransform[ennemiRooms.Count];
        for (int i = 0; i < ennemiRooms.Count; i++)
        {
            ennemiRooms[i].myType = new int[_nb[i]];
            ennemiRooms[i].symbole = new Image[_nb[i]];
            sallesRT[i] = ennemiRooms[i].ui;

            for (int h = 0; h < _nb[i]; h++)
            {

                ennemiRooms[i].myType[h] = _types[index];
                ennemiRooms[i].symbole[h] = _symboles[index];
                index++;
            }
        }
        PoolingDBM();
        cartesManager.DrawCards();
    }

    public void PerdrePvLocal(int wantedRoom, int wantedDamage)
    {
        ennemiRooms[wantedRoom].pv -= wantedDamage;
        float fill = (ennemiRooms[wantedRoom].pv / ennemiRooms[wantedRoom].pvMax) * 100;
        ennemiRooms[wantedRoom].pvText.text = Mathf.RoundToInt(fill).ToString() + " %";

        if (ennemiRooms[wantedRoom].pv <= 0)
        {
            ennemiRooms[wantedRoom].pv = 0;
            ennemiRooms[wantedRoom].isDead = true;
            CancelAction(wantedRoom);
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
            EndCombat();
            Debug.Log("Dracarys");
            boutonSpawn.SetActive(true);
        }
    }

    public void EndCombat()
    {
        Destroy(badGuy);
        ennemiRooms.Clear();
        actionPrevues.Clear();
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
                float fill = (ennemiRooms[i].pv / ennemiRooms[i].pvMax) * 100;
                ennemiRooms[i].pvText.text = Mathf.RoundToInt(fill).ToString() + " %";

                if (ennemiRooms[i].pv >= ennemiRooms[i].pvMax)
                {
                    // salle réparée
                    ennemiRooms[i].pv = ennemiRooms[i].pvMax;
                    ennemiRooms[i].isDead = false;
                    ennemiRooms[i].etat = 1;
                    ennemiRooms[i].timer = Random.Range(8, 16);
                }
            }
            else if (ennemiRooms[i].etat == 1)
            {
                if (ennemiRooms[i].timer <= 0)
                {
                    // ChooseNextAttaque
                    ChooseAction(i);
                    ennemiRooms[i].etat = 2;
                    ennemiRooms[i].isAttacking = true;
                }

            }
            else if (ennemiRooms[i].etat == 2)
            {
                if (ennemiRooms[i].isAttacking == false)
                {
                    // attaque a été lancée, on passe en cooldown
                    ennemiRooms[i].timer = Random.Range(8, 16);
                    ennemiRooms[i].etat = 1;
                }
            }
        }
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
        badGuy.transform.SetParent(myCanvas.transform, false);
        badGuy.SetActive(true);
        boutonSpawn.SetActive(false);
    }

    public void ChooseAction(int _origine)
    {
        var a = IndexActionToDBM();
        actionPrevues[a].timer = Random.Range(10, 20);
        actionPrevues[a].origine = _origine;
        actionPrevues[a].cible = Random.Range(0, 4);
        actionPrevues[a].id = Random.Range(1, 3);
        ennemiRooms[_origine].timer = actionPrevues[a].timer;

        if (actionPrevues[a].id % 2 != 0)
        {
            salleManager.allSalles[actionPrevues[a].cible].isAttacked = true;
            salleManager.ChangeMaterial();
        }
        SortDBMActionByTimer();
    }

    public void MajDBM()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < actionPrevues.Count)
            {
                if (actionPrevues[i].id > 0)
                {
                    if (actionPrevues[i].id % 2 != 0)
                    {
                        DBM_Cible[i].text = "<color=#126A0A>Room " + actionPrevues[i].cible.ToString() + "</color>";
                    }
                    else
                    {
                        DBM_Cible[i].text = "<color=#BC1910>Room " + actionPrevues[i].cible.ToString() + "</color>";
                    }

                    DBM_timer[i].text = actionPrevues[i].timer.ToString("F3") + "s ";
                    if (!DBM_Symbole[i].enabled)
                    {
                        DBM_Symbole[i].enabled = true;
                        DBM_Symbole[i].sprite = ennemiRooms[actionPrevues[i].origine].symbole[0].sprite;
                    }
                }
                else
                {
                    if (DBM_Symbole[i].enabled)
                    {
                        DBM_Cible[i].text = "";
                        DBM_timer[i].text = "";
                        DBM_Symbole[i].enabled = false;
                    }
                }
            }
        }
    }

    public void GestionDesActions()
    {
        for (int i = 0; i < actionPrevues.Count; i++)
        {
            if (actionPrevues[i].id > 0)
            {
                actionPrevues[i].timer -= Time.deltaTime;
                if (actionPrevues[i].id % 2 != 2)
                {
                    if (actionPrevues[i].timer <= 0)
                    {
                        salleManager.allSalles[actionPrevues[i].cible].isAttacked = false;
                        salleManager.ChangeMaterial();
                        salleManager.DamageSurSalle(actionPrevues[i].cible, 00); // DEGATS SET A 0 ATTENTION
                        ennemiRooms[actionPrevues[i].origine].isAttacking = false;
                        actionPrevues[i].id = -1;
                        actionPrevues[i].timer = 50000;
                    }

                }
                else
                {
                    if (actionPrevues[i].timer <= 0)
                    {
                        ennemiRooms[actionPrevues[i].origine].isAttacking = false;
                        actionPrevues[i].id = -1;
                        actionPrevues[i].timer = 50000;
                    }
                }
            }
        }
        SortDBMActionByTimer();
    }

    public void CancelAction(int _indexSalle)
    {
        for (int i = 0; i < actionPrevues.Count; i++)
        {
            if (actionPrevues[i].origine == _indexSalle)
            {
                if (actionPrevues[i].id % 2 != 0)
                {
                    salleManager.allSalles[actionPrevues[i].cible].isAttacked = false;
                    salleManager.ChangeMaterial();
                }
                actionPrevues[i].id = -1;
                actionPrevues[i].timer = 50000;
                i = actionPrevues.Count;
            }
        }
        SortDBMActionByTimer();
    }

    // object pooling pour DBM semble une meilleure solution pour éviter les crash
    // Créer le nombre de DBM correspondant au nombre de la salles adverses
    // On se sert de ID pour définir si l'object DBM est libre ou non de stocker une action :
    // DBM.id = -1 DBM LIBRE
    // DBM.id > 0 DBM A UNE ACTION STOCKEE
    // DBM.id > 0 && DBM.id % 2 == 0 DBM A UNE DEFENSE STOCKEE
    // DBM.id > 0 && DBM.id % 2 != 0 DBM A UNE ATTAQUE STOCKEE

    void PoolingDBM()
    {
        for (int i = 0; i < ennemiRooms.Count; i++)
        {
            var a = new DBM();
            actionPrevues.Add(a);
        }
    }

    int IndexActionToDBM()
    {
        int index = 0;
        for (int i = 0; i < actionPrevues.Count; i++)
        {
            if (actionPrevues[i].id < 0)
            {
                index = i;
                i = actionPrevues.Count;
            }
        }
        return index;
    }

    void SortDBMActionByTimer()
    {
        actionPrevues.Sort((p1, p2) => p1.timer.CompareTo(p2.timer)); // THIS WILL WORKS ?!????
    }

}
