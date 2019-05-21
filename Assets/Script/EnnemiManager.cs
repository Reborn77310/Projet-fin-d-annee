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
    public string textInfos = "";

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
    public bool canBeTarget = true;
    public float projectileReduction = 1;
    public float cannalisationReduction = 1;
    public bool reflect = false;
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
    BattleLog battleLog;

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
    public List<SalleManager.Effets> effetsEnnemi = new List<SalleManager.Effets>();
    public Image[] formule = new Image[6];
    public Animator[] animators;

    private void Awake()
    {
        cartesManager = GetComponent<CartesManager>();
        salleManager = GetComponent<SalleManager>();
        battleLog = GetComponent<BattleLog>();
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
            animators[i].SetBool("cooldown", true);
        }
    }

    public void ApplyFormule(Sprite[] s)
    {
        for (int i = 0; i < formule.Length; i++)
        {
            formule[i].sprite = s[i];
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

    public void PerdrePvLocal(int wantedRoom, float wantedDamage)
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

    public void PerdrePvGlobal(float numberOfPv)
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
                ennemiRooms[i].pvText.text = "<color=#4061BA>" + Mathf.RoundToInt(fill).ToString() + " % </color>";

                if (ennemiRooms[i].pv >= ennemiRooms[i].pvMax)
                {
                    ennemiRooms[i].pvText.text = "<color=#BC1910>" + Mathf.RoundToInt(fill).ToString() + " % </color>";
                    // salle réparée
                    ennemiRooms[i].pv = ennemiRooms[i].pvMax;
                    ennemiRooms[i].isDead = false;
                    ennemiRooms[i].etat = 1;
                    ennemiRooms[i].timer = Random.Range(8, 16);
                    animators[i].SetBool("cooldown", true);
                }
            }
            else if (ennemiRooms[i].etat == 1)
            {
                if (ennemiRooms[i].timer <= 0)
                {
                    animators[i].SetBool("cooldown", false);
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
                    animators[i].SetBool("cooldown", true);
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

    public int RandomCible()
    {
        int toReturn = Random.Range(0, 4);
        while (!salleManager.allSalles[toReturn].canBeTarget)
        {
            toReturn = Random.Range(0, 4);
        }
        return toReturn;
    }

    public void ChooseAction(int _origine)
    {

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
                    actionPrevues[a].cible = RandomCible();
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
                    actionPrevues[a].cible = RandomCible();
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
                actionPrevues[a].cible = RandomCible();
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
                    actionPrevues[a].cible = RandomCible();
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
                    actionPrevues[a].cible = RandomCible();
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
                actionPrevues[a].cible = RandomCible();
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
                actionPrevues[a].cible = RandomCible();
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
                actionPrevues[a].cible = RandomCible();
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
                actionPrevues[a].cible = RandomCible();
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
            if (AttaquesPool(_origine).Length > 0 && salleManager.allSalles[3].etat != 2)
            {
                var temp = AttaquesPool(_origine);
                int rnd = Random.Range(0, temp.Length);
                var a = IndexActionToDBM();
                actionPrevues[a].timer = Random.Range(10, 20);
                actionPrevues[a].origine = _origine;
                actionPrevues[a].cible = 3;
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
                actionPrevues[a].cible = RandomCible();
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
                    if (actionPrevues[i].textInfos == "")
                    {
                        SetTextInDBM(i);
                        print("a");
                    }
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

        battleLog.ChangeTextInfosADV(actionPrevues[0].textInfos);
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
                    actionPrevues[i].textInfos = "";
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
                actionPrevues[i].textInfos = "";
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
        if (ennemiRooms.Count == 3)
        {
            DBM_Symbole[3].text ="";
            DBM_Cible[3].text ="";
            DBM_timer[3].text ="";
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
        if (id == 1 || id == 4 || id == 7 || id == 8 || id == 9)
        {
            string chemin = "Prefabs/FeedbackActionsEnnemisSurNest/";
            var transformParent = salleManager.allSalles[actionPrevues[i].cible].SpawnParticleFeedback.transform;
            GameObject wantedThing = Resources.Load<GameObject>(chemin + id);
            var go = Instantiate(wantedThing, transformParent.position, transformParent.rotation, transformParent);
            salleManager.allSalles[actionPrevues[i].cible].ActualFeedbackOnMe = go;
        }

        //print(id + " " + actionPrevues[i].origine);
        if (id == 1) // Missile 1
        {
            print("Missile 1 " + actionPrevues[i].origine);
            // PROJECTILE
            salleManager.DamageSurSalle(actionPrevues[i].cible, 25);
        }
        else if (id == 2) // Missile 2
        {
            print("Missile 2 " + actionPrevues[i].origine);
            // PROJECTILE
            salleManager.DamageSurSalle(actionPrevues[i].cible, 45);
        }
        else if (id == 3) // Singularité empêche overdrive
        {
            print("Singularity " + actionPrevues[i].origine);
            salleManager.allSalles[actionPrevues[i].cible].canOverdrive = false;
            string[] a = new string[] { "DURATION", "ELECTRONIC" };
            salleManager.AddEffets(18, "Singularity", a, actionPrevues[i].cible, 0);
        }
        else if (id == 4) // Armagedon
        {
            // PROJECTILE
            print("Armagedon " + actionPrevues[i].origine);
            salleManager.DamageSurSalle(actionPrevues[i].cible, 70);
            // durability -2
        }
        else if (id == 5) // Poinçonneuse
        {
            // PROJECTILE
            print("Poinçonneuse " + actionPrevues[i].origine);
            salleManager.DamageSurSalle(actionPrevues[i].cible, 15);
            if (salleManager.allSalles[actionPrevues[i].cible].pv <= 15)
            {
                // Enlever la carte de gauche dans la main
            }
        }
        else if (id == 6) // Grappin
        {
            // Brouille le radar
            print("Grappin " + actionPrevues[i].origine);
            string[] a = new string[] { "DURATION", "PROJECTILE" };
            salleManager.AddEffets(10, "Grappin", a, actionPrevues[i].cible, 0);
        }
        else if (id == 7) // Drone incendiaire
        {
            print("Drone incendiaire " + actionPrevues[i].origine);
            // Le drone
            // DRONE CHARGE
        }
        else if (id == 8) // Tourelle DASSAULT
        {
            print("dassault " + actionPrevues[i].origine);
            // PROJECTILE && CANALISATION
            if (salleManager.allSalles[actionPrevues[i].cible].etat == 1)
            {
                salleManager.DamageSurSalle(actionPrevues[i].cible, 10);
                salleManager.DamageSurSalle(actionPrevues[i].cible, 10);
                salleManager.DamageSurSalle(actionPrevues[i].cible, 10);
                salleManager.DamageSurSalle(actionPrevues[i].cible, 10);
                salleManager.DamageSurSalle(actionPrevues[i].cible, 10);
            }
            else
            {
                salleManager.DamageSurSalle(actionPrevues[i].cible, 5);
                salleManager.DamageSurSalle(actionPrevues[i].cible, 5);
                salleManager.DamageSurSalle(actionPrevues[i].cible, 5);
                salleManager.DamageSurSalle(actionPrevues[i].cible, 5);
                salleManager.DamageSurSalle(actionPrevues[i].cible, 5);
            }

        }
        else if (id == 9) // Fumée
        {
            print("Fumée " + actionPrevues[i].origine);
            // declencher smoke ajouter perte de durabilité
            string[] a = new string[] { "DURATION", "CHIMIC" };
            salleManager.AddEffets(20, "Smoke", a, actionPrevues[i].cible, 0);
            salleManager.allEffets[salleManager.allEffets.Count - 1].Feedback = salleManager.allSalles[actionPrevues[i].cible].ActualFeedbackOnMe;
        }
        else if (id == -1) // Drone anti-projectile
        {
            print("anti-proj " + actionPrevues[i].origine);
            string[] a = new string[] { "DURATION", "DRONE" };
            ennemiRooms[actionPrevues[i].cible].projectileReduction -= 0.5f;
            AddEffets(15, "Terra", a, actionPrevues[i].cible, -0.5f);
        }
        else if (id == -2)  // Reflect
        {
            print("reflect " + actionPrevues[i].origine);
            string[] a = new string[] { "DURATION", "GAMMA" };
            ennemiRooms[actionPrevues[i].cible].reflect = true;
            AddEffets(10, "Reflect", a, actionPrevues[i].cible, 0);
        }
        else if (id == -3)  // Bouclier anti-canalisation
        {
            print("anti-cana " + actionPrevues[i].origine);
            string[] a = new string[] { "DURATION", "ELECTRONIC" };
            ennemiRooms[actionPrevues[i].cible].cannalisationReduction -= 1f;
            AddEffets(10, "Anti-cannalisation", a, actionPrevues[i].cible, -1f);
        }
        else if (id == -4)  // Cloaking
        {
            print("cloaking " + actionPrevues[i].origine);
            string[] a = new string[] { "DURATION", "GAMMA" };
            for (int h = 0; h < ennemiRooms.Count; h++)
            {
                ennemiRooms[h].canBeTarget = false;
            }
            AddEffets(5, "Cloaking", a, actionPrevues[i].cible, 0);
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

    public void AddEffets(float _duration, string _name, string[] _tags, int _salle, float _value)
    {
        SalleManager.Effets a = new SalleManager.Effets();

        a.duration = _duration;
        a.name = _name;
        a.tags = _tags;
        a.salle = _salle;
        a.value = _value;

        effetsEnnemi.Add(a);
    }

    public void GestionDesEffets()
    {
        for (int i = 0; i < effetsEnnemi.Count; i++)
        {
            effetsEnnemi[i].duration -= Time.deltaTime;
            if (effetsEnnemi[i].duration <= 0)
            {
                RemoveEffets(i);
                effetsEnnemi.RemoveAt(i);
            }
        }
    }

    public void RemoveEffets(int i)
    {
        if (effetsEnnemi[i].name == "Terra")
        {
            ennemiRooms[effetsEnnemi[i].salle].projectileReduction += effetsEnnemi[i].value;
        }
        else if (effetsEnnemi[i].name == "Reflect")
        {
            ennemiRooms[effetsEnnemi[i].salle].reflect = false;
        }
        else if (effetsEnnemi[i].name == "Anti-cannalisation")
        {
            ennemiRooms[effetsEnnemi[i].salle].cannalisationReduction += effetsEnnemi[i].value;
        }
        else if (effetsEnnemi[i].name == "Cloaking")
        {
            for (int h = 0; h < ennemiRooms.Count; h++)
            {
                ennemiRooms[h].canBeTarget = true;
            }
        }
    }

    public void SetTextInDBM(int idDBM)
    {
        int id = actionPrevues[idDBM].id;
        if (id == 1) // Missile 1
        {
            actionPrevues[idDBM].textInfos = "<b><color=#E6742E>PROJECTILE</color></b>" + "\n" + "Deals <color=#7841BB>25 damage</color>.";
        }
        else if (id == 2) // Missile 2
        {
            actionPrevues[idDBM].textInfos = "<b><color=#E6742E>PROJECTILE</color></b>" + "\n" + "Deals <color=#7841BB>45 damage</color>.";
        }
        else if (id == 3) // Singularité empêche overdrive
        {
            actionPrevues[idDBM].textInfos = "<b><color=#B97D31>ELECTRONIC</color></b>" + "\n" + "Prevents the use of overdrive effects. Last 18s.";
        }
        else if (id == 4) // Armagedon
        {
            actionPrevues[idDBM].textInfos = "<b><color=#E6742E>GAMMA</color></b>" + "\n" + "Deals <color=#7841BB>70 damage</color>. Reduces card durability of 2.";
        }
        else if (id == 5) // Poinçonneuse
        {
            actionPrevues[idDBM].textInfos = "<b><color=#E6742E>MINES</color></b>" + "\n" + "Deals <color=#7841BB>15 damage</color>. If a room is down as a result of this action, discard your leftmost card.";
        }
        else if (id == 6) // Grappin
        {
            actionPrevues[idDBM].textInfos = "<b><color=#B97D31>DURATION, PROJECTILE</color></b>" + "\n" + "Blurs your radar. Last 10s.";
        }
        else if (id == 7) // Drone incendiaire
        {
            actionPrevues[idDBM].textInfos = "<b><color=#E6742E>DRONE</color></b>" + "\n" + "Deals <color=#7841BB>35 damage</color>. Set the room on fire for 10s.";
        }
        else if (id == 8) // Tourelle DASSAULT
        {
            actionPrevues[idDBM].textInfos = "<b><color=#E6742E>PROJECTILE, CANALIZATION</color></b>" + "\n" + "Deals <color=#7841BB>5 damage</color> 5 times. Deals <color=#7841BB>5 more damage</color> each time if target room is in cooldown.";
        }
        else if (id == 9) // Fumée
        {
            actionPrevues[idDBM].textInfos = "<b><color=#B97D31>DURATION, CHIMIC</color></b>" + "\n" + "Smokes. Last 20s. Reduces card durability of 1.";
        }
        else if (id == -1) // Drone anti-projectile
        {
            actionPrevues[idDBM].textInfos = "<b><color=#6289F3>DURATION, DRONE</color></b>" + "\n" + "Reduces projectiles effectiveness against target for 50%. Last 15s.";
        }
        else if (id == -2)  // Reflect
        {
            actionPrevues[idDBM].textInfos = "<b><color=#6289F3>DURATION, GAMMA</color></b>" + "\n" + "Duplicates offensive actions against target. Last 10s.";
        }
        else if (id == -3)  // Bouclier anti-canalisation
        {
            actionPrevues[idDBM].textInfos = "<b><color=#6289F3>DURATION</color></b>" + "\n" + "Reduces canalization effectiveness against target for 100%. Last 10s.";
        }
        else if (id == -4)  // Cloaking
        {
            actionPrevues[idDBM].textInfos = "<b><color=#6289F3>DURATION, GAMMA</color></b>" + "\n" + "<size=120%><b>The Voydroc's</b></size> rooms can't be targeted. Last 5.";
        }
    }

}
