using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using System.Linq;

public class DBM
{
    public int origine; // index de la salle d'origine dans ennemiRooms
    public int cible; // index de la cible
    public float timer = 50000; // Temps restant avant lancement de l'action
    public int id = 0; //Correspond à l'id de l'action qui servira à appeler la fonction correspondante.
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
    public int[] actions;
    public float pv = 100;
    public float pvMax = 100;
    public TextMeshProUGUI pvText;
    public bool isDead = false;
    public float timer;
    public int etat = 1;
    public bool isAttacking = false;
    public int iteration = 0;
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

    public GameObject horsCombat; // Les éléments d'ui hors combat
    public GameObject combat; // Les éléments d'ui en combat

    public GameObject prefabADV;
    public GameObject boutonSpawn;
    public GameObject NEST;

    public TextMeshProUGUI[] DBM_Symbole;
    public TextMeshProUGUI[] DBM_Cible;
    public TextMeshProUGUI[] DBM_timer;

    public TextMeshProUGUI pvTotauxText;
    public TextMeshProUGUI infosNext;

    public List<DBM> actionPrevues = new List<DBM>();
    public RectTransform[] sallesRT;

    private void Awake()
    {
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

    public void RecupActions(int[] _nb, int[] actions)
    {
        int index = 0;
        for (int i = 0; i < ennemiRooms.Count; i++)
        {
            ennemiRooms[i].actions = new int[_nb[i]];

            for (int h = 0; h < _nb[i]; h++)
            {
                ennemiRooms[i].actions[h] = actions[index];
                index++;
            }
        }
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
        NEST.SetActive(false);
        pvTotauxText.gameObject.SetActive(false);
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
        NEST.SetActive(true);
        badGuy = GameObject.Instantiate(prefabADV);
        badGuy.transform.SetParent(myCanvas.transform, false);
        badGuy.SetActive(true);
        boutonSpawn.SetActive(false);
    }

    public void ChooseAction(int _origine)
    {
        print(_origine);
        int type = int.Parse(ennemiRooms[_origine].symbole[0].sprite.name.Substring(5, 1));
        if (type == 0)
        {
            if (NombreAttaques() < 2)
            {
                var temp = AttaquesPool(_origine);
                if (temp.Length == 0)
                {
                    ennemiRooms[_origine].timer = 5;
                    ennemiRooms[_origine].iteration += 1;
                }
                else if (temp.Length == 1)
                {
                    var a = IndexActionToDBM();
                    actionPrevues[a].timer = Random.Range(10, 20);
                    actionPrevues[a].origine = _origine;
                    actionPrevues[a].cible = Random.Range(0, 4);
                    actionPrevues[a].id = temp[0];
                    ennemiRooms[_origine].timer = actionPrevues[a].timer;

                    salleManager.allSalles[actionPrevues[a].cible].isAttacked = true;
                    salleManager.ChangeMaterial();

                    ennemiRooms[_origine].etat = 2;
                    ennemiRooms[_origine].isAttacking = true;
                    ennemiRooms[_origine].iteration = 0;
                }
                else
                {
                    int rnd = Random.Range(0, temp.Length);
                    var a = IndexActionToDBM();
                    actionPrevues[a].timer = Random.Range(10, 20);
                    actionPrevues[a].origine = _origine;
                    actionPrevues[a].cible = Random.Range(0, 4);
                    actionPrevues[a].id = temp[rnd];
                    ennemiRooms[_origine].timer = actionPrevues[a].timer;

                    salleManager.allSalles[actionPrevues[a].cible].isAttacked = true;
                    salleManager.ChangeMaterial();

                    ennemiRooms[_origine].etat = 2;
                    ennemiRooms[_origine].isAttacking = true;
                    ennemiRooms[_origine].iteration = 0;
                }
            }
            else if (ennemiRooms[_origine].iteration >= 5)
            {
                int rnd = Random.Range(0, ennemiRooms[_origine].actions.Length);
                var a = IndexActionToDBM();
                actionPrevues[a].timer = Random.Range(10, 20);
                actionPrevues[a].origine = _origine;
                actionPrevues[a].cible = Random.Range(0, 4);
                actionPrevues[a].id = ennemiRooms[_origine].actions[rnd];
                ennemiRooms[_origine].timer = actionPrevues[a].timer;

                if (actionPrevues[a].id > 0)
                {
                    salleManager.allSalles[actionPrevues[a].cible].isAttacked = true;
                    salleManager.ChangeMaterial();
                }

                ennemiRooms[_origine].etat = 2;
                ennemiRooms[_origine].isAttacking = true;
                ennemiRooms[_origine].iteration = 0;

            }
            else
            {
                ennemiRooms[_origine].timer = 5;
                ennemiRooms[_origine].iteration += 1;
            }
        }
        else if (type == 1)
        {
            int b = NombreDefenses();
            if (b < 2)
            {
                var temp = DefensesPool(_origine);
                if (temp.Length == 0)
                {
                    ennemiRooms[_origine].timer = 5;
                    ennemiRooms[_origine].iteration += 1;
                }
                else if (temp.Length == 1)
                {
                    var a = IndexActionToDBM();
                    actionPrevues[a].timer = Random.Range(10, 20);
                    actionPrevues[a].origine = _origine;
                    actionPrevues[a].cible = Random.Range(0, 4);
                    actionPrevues[a].id = temp[0];
                    ennemiRooms[_origine].timer = actionPrevues[a].timer;

                    ennemiRooms[_origine].etat = 2;
                    ennemiRooms[_origine].isAttacking = true;
                    ennemiRooms[_origine].iteration = 0;
                }
                else
                {
                    int rnd = Random.Range(0, temp.Length);
                    var a = IndexActionToDBM();
                    actionPrevues[a].timer = Random.Range(10, 20);
                    actionPrevues[a].origine = _origine;
                    actionPrevues[a].cible = Random.Range(0, 4);
                    actionPrevues[a].id = temp[rnd];
                    ennemiRooms[_origine].timer = actionPrevues[a].timer;

                    ennemiRooms[_origine].etat = 2;
                    ennemiRooms[_origine].isAttacking = true;
                    ennemiRooms[_origine].iteration = 0;
                }
            }
            else if (ennemiRooms[_origine].iteration >= 5)
            {
                int rnd = Random.Range(0, ennemiRooms[_origine].actions.Length);
                var a = IndexActionToDBM();
                actionPrevues[a].timer = Random.Range(10, 20);
                actionPrevues[a].origine = _origine;
                actionPrevues[a].cible = Random.Range(0, 4);
                actionPrevues[a].id = ennemiRooms[_origine].actions[rnd];
                ennemiRooms[_origine].timer = actionPrevues[a].timer;

                if (actionPrevues[a].id > 0)
                {
                    salleManager.allSalles[actionPrevues[a].cible].isAttacked = true;
                    salleManager.ChangeMaterial();
                }

                ennemiRooms[_origine].etat = 2;
                ennemiRooms[_origine].isAttacking = true;
                ennemiRooms[_origine].iteration = 0;

            }
            else
            {
                ennemiRooms[_origine].timer = 5;
                ennemiRooms[_origine].iteration += 1;
            }
        }
        else if (type == 2)
        {
            if (NombreAttaques() == 0 && AttaquesPool(_origine).Length > 0)
            {
                var temp = AttaquesPool(_origine);
                int rnd = Random.Range(0, temp.Length);
                var a = IndexActionToDBM();
                actionPrevues[a].timer = Random.Range(10, 20);
                actionPrevues[a].origine = _origine;
                actionPrevues[a].cible = Random.Range(0, 4);
                actionPrevues[a].id = temp[rnd];
                ennemiRooms[_origine].timer = actionPrevues[a].timer;

                salleManager.allSalles[actionPrevues[a].cible].isAttacked = true;
                salleManager.ChangeMaterial();

                ennemiRooms[_origine].etat = 2;
                ennemiRooms[_origine].isAttacking = true;
                ennemiRooms[_origine].iteration = 0;
            }
            else if (NombreDefenses() == 0 && DefensesPool(_origine).Length > 0)
            {
                var temp = DefensesPool(_origine);
                int rnd = Random.Range(0, temp.Length);
                var a = IndexActionToDBM();
                actionPrevues[a].timer = Random.Range(10, 20);
                actionPrevues[a].origine = _origine;
                actionPrevues[a].cible = Random.Range(0, 4);
                actionPrevues[a].id = temp[rnd];
                ennemiRooms[_origine].timer = actionPrevues[a].timer;

                ennemiRooms[_origine].etat = 2;
                ennemiRooms[_origine].isAttacking = true;
                ennemiRooms[_origine].iteration = 0;
            }
            else if (ennemiRooms[_origine].iteration >= 3)
            {
                int rnd = Random.Range(0, ennemiRooms[_origine].actions.Length);
                var a = IndexActionToDBM();
                actionPrevues[a].timer = Random.Range(10, 20);
                actionPrevues[a].origine = _origine;
                actionPrevues[a].cible = Random.Range(0, 4);
                actionPrevues[a].id = ennemiRooms[_origine].actions[rnd];
                ennemiRooms[_origine].timer = actionPrevues[a].timer;

                if (actionPrevues[a].id > 0)
                {
                    salleManager.allSalles[actionPrevues[a].cible].isAttacked = true;
                    salleManager.ChangeMaterial();
                }

                ennemiRooms[_origine].etat = 2;
                ennemiRooms[_origine].isAttacking = true;
                ennemiRooms[_origine].iteration = 0;
            }
            else
            {
                ennemiRooms[_origine].timer = 5;
                ennemiRooms[_origine].iteration += 1;
            }
        }
        else if (type == 4)
        {
            if (AttaquesPool(_origine).Length > 0 && !salleManager.allSalles[3].isReparing)
            {
                var temp = AttaquesPool(_origine);
                int rnd = Random.Range(0, temp.Length);
                var a = IndexActionToDBM();
                actionPrevues[a].timer = Random.Range(10, 20);
                actionPrevues[a].origine = _origine;
                actionPrevues[a].cible = Random.Range(0, 4);
                actionPrevues[a].id = temp[rnd];
                ennemiRooms[_origine].timer = actionPrevues[a].timer;

                salleManager.allSalles[actionPrevues[a].cible].isAttacked = true;
                salleManager.ChangeMaterial();

                ennemiRooms[_origine].etat = 2;
                ennemiRooms[_origine].isAttacking = true;
                ennemiRooms[_origine].iteration = 0;
            }
            else if (ennemiRooms[_origine].iteration >= 3)
            {
                int rnd = Random.Range(0, ennemiRooms[_origine].actions.Length);
                var a = IndexActionToDBM();
                actionPrevues[a].timer = Random.Range(10, 20);
                actionPrevues[a].origine = _origine;
                actionPrevues[a].cible = Random.Range(0, 4);
                actionPrevues[a].id = ennemiRooms[_origine].actions[rnd];
                ennemiRooms[_origine].timer = actionPrevues[a].timer;

                if (actionPrevues[a].id > 0)
                {
                    salleManager.allSalles[actionPrevues[a].cible].isAttacked = true;
                    salleManager.ChangeMaterial();
                }

                ennemiRooms[_origine].etat = 2;
                ennemiRooms[_origine].isAttacking = true;
                ennemiRooms[_origine].iteration = 0;
            }
            else
            {
                ennemiRooms[_origine].timer = 5;
                ennemiRooms[_origine].iteration += 1;
            }
        }
        SortDBMActionByTimer();
    }

    public void MajDBM()
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < actionPrevues.Count)
            {
                if (actionPrevues[i].id != 0)
                {
                    if (actionPrevues[i].id > 0)
                    {
                        DBM_Cible[i].text = "<color=#126A0A>Room " + (actionPrevues[i].cible + 1).ToString() + "</color>";
                    }
                    else
                    {
                        DBM_Cible[i].text = "<color=#BC1910>Room " + (actionPrevues[i].cible + 1).ToString() + "</color>";
                    }

                    DBM_timer[i].text = actionPrevues[i].timer.ToString("F3") + "s ";
                    DBM_Symbole[i].text = (actionPrevues[i].origine + 1).ToString();

                }
                else
                {
                    if (DBM_Symbole[i].text != "")
                    {
                        DBM_Cible[i].text = "";
                        DBM_timer[i].text = "";
                        DBM_Symbole[i].text = "";
                    }
                }
            }
        }
    }

    public void GestionDesActions()
    {
        for (int i = 0; i < actionPrevues.Count; i++)
        {
            if (actionPrevues[i].id != 0)
            {
                actionPrevues[i].timer -= Time.deltaTime;
                if (actionPrevues[i].timer <= 0)
                {
                    if (actionPrevues[i].id > 0)
                    {
                        salleManager.allSalles[actionPrevues[i].cible].isAttacked = false;
                        salleManager.ChangeMaterial();
                    }

                    ActionsEnnemies(actionPrevues[i].id, i);
                    ennemiRooms[actionPrevues[i].origine].isAttacking = false;
                    actionPrevues[i].id = 0;
                    actionPrevues[i].timer = 50000;
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
                if (actionPrevues[i].id > 0)
                {
                    salleManager.allSalles[actionPrevues[i].cible].isAttacked = false;
                    salleManager.ChangeMaterial();
                }
                actionPrevues[i].id = 0;
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
            if (actionPrevues[i].id == 0)
            {
                index = i;
                i = actionPrevues.Count;
            }
        }
        return index;
    }

    void SortDBMActionByTimer()
    {
        actionPrevues.Sort((p1, p2) => p1.timer.CompareTo(p2.timer));
    }

    public void ActionsEnnemies(int id, int i)
    {
        if (id == 1)
        {
            salleManager.DamageSurSalle(actionPrevues[i].cible, 100); // DEGATS SET A 0 ATTENTION
        }
        else if (id == 2)
        {
            salleManager.DamageSurSalle(actionPrevues[i].cible, 100); // DEGATS SET A 0 ATTENTION
        }
        else if (id == -1)
        {
            // Defense
            print("defense");
        }
        else if (id == -2)
        {
            // Defense+
            print("defense");
        }
    }

    public int NombreAttaques()
    {
        int toReturn = 0;
        for (int i = 0; i < actionPrevues.Count; i++)
        {
            if (actionPrevues[i].id > 0)
            {
                toReturn += 1;
            }
        }
        return toReturn;
    }

    public int NombreDefenses()
    {
        int toReturn = 0;
        for (int i = 0; i < actionPrevues.Count; i++)
        {
            if (actionPrevues[i].id < 0)
            {
                toReturn += 1;
            }
        }
        return toReturn;
    }

    public int[] AttaquesPool(int idSalle)
    {
        int[] toReturn;
        int index = 0;
        for (int i = 0; i < ennemiRooms[idSalle].actions.Length; i++)
        {
            if (ennemiRooms[idSalle].actions[i] > 0)
            {
                index++;
            }
        }
        toReturn = new int[index];
        index = 0;
        for (int i = 0; i < ennemiRooms[idSalle].actions.Length; i++)
        {
            if (ennemiRooms[idSalle].actions[i] > 0)
            {
                toReturn[index] = ennemiRooms[idSalle].actions[i];
                index++;
            }
        }
        return toReturn;
    }

    public int[] DefensesPool(int idSalle)
    {
        int[] toReturn;
        int index = 0;
        for (int i = 0; i < ennemiRooms[idSalle].actions.Length; i++)
        {
            if (ennemiRooms[idSalle].actions[i] < 0)
            {
                index++;
            }
        }
        toReturn = new int[index];
        index = 0;
        for (int i = 0; i < ennemiRooms[idSalle].actions.Length; i++)
        {
            if (ennemiRooms[idSalle].actions[i] < 0)
            {
                toReturn[index] = ennemiRooms[idSalle].actions[i];
                index++;
            }
        }
        return toReturn;
    }

    public void CheckEquipementElectronique()
    {
        // if salle symbole spé
        // if sall
    }
}
