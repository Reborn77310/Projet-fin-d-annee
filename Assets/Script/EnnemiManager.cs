using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnnemiRooms
{
    public GameObject myRoom;
    public int myType;
    public Image pictoFormule;
    public float myPdv = 100;
    public Image myImagePv;
    public bool isDead = false;
    public float timer;
    public int etat = 1;
    public int salleFocus;
    public EnnemiRooms(float _timer)
    {
        timer = _timer;
    }
}

// public class Spawns
// {
//     public GameObject go;
//     public int id;
//     public Image myHP;
//     public Text myText;
//     public int myPV;
//     public EnnemiRooms MyChilds = new EnnemiRooms();


//     public Spawns(GameObject _go, int _id)
//     {
//         go = _go;
//         id = _id;
//         myHP = go.transform.GetChild(6).GetComponent<Image>();
//         myPV = 300;
//         myHP.fillAmount = myPV;
//         myText = myHP.transform.GetChild(0).GetComponent<Text>();
//         myText.text = "Hp : " + myPV;

//         var childs = go.transform.GetChild(5);
//         for (int i = 0; i < 4; i++)
//         {
//             MyChilds.myRooms[i] = childs.transform.GetChild(i).gameObject;
//             int randomRange = Random.Range(0, 3);
//             MyChilds.myTypes[i] = randomRange;
//             MyChilds.myPdv[i] = 100;
//             MyChilds.isDead[i] = false;
//             MyChilds.myText[i] = _go.transform.GetChild(i).GetComponent<Text>();
//             MyChilds.myRooms[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/Picto" + randomRange);
//             MyChilds.myText[i].text = "Salle " + i + " = " + MyChilds.myPdv[i] + " pdv.";
//             MyChilds.etat[i] = 1;
//             MyChilds.timer[i] = Random.Range(1, 4) + i * 3; // s0 : 1,4   s1 : 4,7  s2 : 7,11  s3 : 10,13
//         }
//     }
// }

public class EnnemiManager : MonoBehaviour
{
    public List<EnnemiRooms> ennemiRooms = new List<EnnemiRooms>();
    public Text[] intentionMechantes;
    public SalleManager salleManager;
    public int pv;
    public GameObject badGuy;
    public GameObject badGuyPrefab;
    public Canvas myCanvas;

    void Start()
    {
        SpawnMob(4);
    }

    void Update() {
        CheckPdv();
        GestionDesSalles();
    }

    public void SpawnMob(int nbSalles)
    {
        badGuy = GameObject.Instantiate(badGuyPrefab, myCanvas.transform, false);
        intentionMechantes = badGuy.transform.GetChild(1).GetComponentsInChildren<Text>();

        for (int i = 0; i < nbSalles; i++)
        {
            intentionMechantes[i].text = "";
            EnnemiRooms a = new EnnemiRooms((Random.Range(1,4)+i*3));
            int randomRange = Random.Range(0, 3);
            a.myType = randomRange;
            a.pictoFormule = badGuy.transform.GetChild(3).GetChild(i).GetComponent<Image>();
            a.pictoFormule.sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/Picto" + randomRange);
            a.myImagePv = badGuy.transform.GetChild(0).GetChild(i).GetComponent<Image>();
            ennemiRooms.Add(a);
        }
        pv = 300;
    }

    public void PerdrePvLocal(int wantedRoom, int wantedDamage)
    {
        ennemiRooms[wantedRoom].myPdv -= wantedDamage;
        float fill = ennemiRooms[wantedRoom].myPdv / 100;
        ennemiRooms[wantedRoom].myImagePv.fillAmount = ennemiRooms[wantedRoom].myPdv / 100;

        if (ennemiRooms[wantedRoom].myPdv <= 0)
        {
            ennemiRooms[wantedRoom].isDead = true;
            intentionMechantes[wantedRoom].text = "";
            ennemiRooms[wantedRoom].timer = 20;
            ennemiRooms[wantedRoom].etat = 0;
        }
    }

    public void PerdrePvGlobal(int numberOfPv)
    {
        pv -= numberOfPv;
        badGuy.transform.GetChild(2).GetComponent<Image>().fillAmount = pv / 300;
        badGuy.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = pv.ToString();
    }


    public void CheckPdv()
    {
        if (pv <= 0)
        {
            Time.timeScale = 0;
            Debug.Log("Dracarys");
            pv = 1;
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
                intentionMechantes[i].text = "Repair..." + "\n" + ennemiRooms[i].timer;
                if (ennemiRooms[i].timer <= 0)
                {
                    // salle réparée
                    ennemiRooms[i].isDead = false;
                    ennemiRooms[i].myPdv = 100;
                    ennemiRooms[i].etat = 1;
                    intentionMechantes[i].text = "";
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
                intentionMechantes[i].text = "Next Attack" + "\n" + ennemiRooms[i].timer + "\n" + "Salle " + ennemiRooms[i].salleFocus.ToString();
                if (ennemiRooms[i].timer <= 0)
                {
                    // attaque lancée
                    salleManager.DamageSurSalle(ennemiRooms[i].salleFocus, 50);
                    intentionMechantes[i].text = "";
                    // cooldown
                    ennemiRooms[i].timer = Random.Range(8, 16);
                    ennemiRooms[i].etat = 1;
                }
            }
        }
    }

    public int[] GiveInfosForDraw()
    {
        int nbSalleAlive = 0;
        for (int i = 0; i < ennemiRooms.Count; i++)
        {
            if (!ennemiRooms[i].isDead)
            {
                nbSalleAlive++;
            }
        }

        int[] typesToDraw = new int[nbSalleAlive];
        int j = 0;
        for (int h = 0; h < ennemiRooms.Count; h++)
        {
            if (!ennemiRooms[h].isDead)
            {
                typesToDraw[j] = ennemiRooms[h].myType;
                j++;
            }
            
        }
        return typesToDraw;
    }
}
