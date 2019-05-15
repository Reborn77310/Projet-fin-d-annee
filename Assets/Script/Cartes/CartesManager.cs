﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class CartesManager : MonoBehaviour
{
    public class Cartes
    {
        public int id; // L'index dans la liste à laquelle elle appartient (allCards ou onModule)
        public int cartesTypes; // 0,1,2 = types basiques // 10+ = types spéciaux
        public Sprite illu;
        public Sprite picto;
        public GameObject go;

        // A SETUP :
        public int durability;
        public GameObject prefabZoneSelection;
        public int rarity;
        public int overdriveEffect;


        public Cartes(int _id, int _cartesTypes)
        {
            id = _id;
            cartesTypes = _cartesTypes;
            illu = Resources.Load<Sprite>("Sprites/Cartes/V5/Carte" + _cartesTypes);
            picto = Resources.Load<Sprite>("Sprites/Cartes/Picto/Picto" + _cartesTypes);
            prefabZoneSelection = Resources.Load("Prefabs/Radar/ZoneSelection/Ciblage_ADV") as GameObject;
        }
    }
    [Header("Lists")]
    public List<Cartes> allCards = new List<Cartes>();
    //public List<Cartes> onModules = new List<Cartes>();
    CartesButtons[] cartesButtonsScripts = new CartesButtons[3];

    [Header("Other")]
    public GameObject prefabCarte;
    public GameMaster gameMaster;
    public Canvas canvas;
    public float drawTimer;
    public Image scannerUI;
    public Text ChangeBool;
    public EnnemiManager ennemiManager;
    public Image grilleRadar;
    public static bool PhaseLente = true;

    #region Initialisation
    void Awake()
    {
        scannerUI.GetComponent<Animator>().SetFloat("speedDraw", 1 / (drawTimer - 1));
    }

    void Start()
    {
        DrawCards();
        scannerUI.gameObject.SetActive(false);
    }


    #endregion

    #region Actions
    public void PlayACardOnModule(int id, ModuleManager mm)
    {
        if (!PhaseLente)
        {
            HandToModule(id, mm);
            if (mm.cartesModule.Count == 3)
            {
                ThirdCardAction(mm);
            }
            SortCartes();
        }
        else
        {

            if (mm.cartesModule.Count < 2)
            {
                var c = allCards[id];
                mm.cartesModule.Add(c);
                mm.MyModules[mm.cartesModule.Count - 1].GetComponent<Image>().sprite = mm.cartesModule[mm.cartesModule.Count - 1].illu;
                SortCartes();
            }

        }
    }



    void ThirdCardAction(ModuleManager mm)
    {
        mm.MyModules[2].transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("MiniUi/CadresCartes_0");
        gameMaster.LancerActionJoueur(mm);
    }

    #endregion

    #region Gestion Des Cartes
    public void DrawCards()
    {
        if (!PhaseLente)
        {
            int[] toDraw = ennemiManager.GiveInfosForDraw();
            for (int i = 0; i < toDraw.Length; i++)
            {
                if (!CheckHandisFull())
                {
                    AjouterUneCarteDansLaMain(1, toDraw[i]);
                }
            }
            StartCoroutine("Draw");
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (!CheckHandisFull())
                {
                    AjouterUneCarteDansLaMain(1, i);
                }
            }
        }
    }

    IEnumerator Draw()
    {
        yield return new WaitForSeconds(drawTimer);
        DrawCards();
        yield break;
    }

    public void AjouterUneCarteDansLaMain(int cardsToDraw, int carteType)
    {
        for (int i = 0; i < cardsToDraw; i++)
        {
            GameObject a = Instantiate(prefabCarte, Vector3.zero, Quaternion.identity, canvas.transform);
            a.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/V5/Carte" + carteType);
            CartesButtons cb = a.GetComponent<CartesButtons>();
            cb.id = allCards.Count + i;


            Cartes c = new Cartes(allCards.Count + i, carteType);
            c.go = a;
            allCards.Add(c);
        }
        SortCartes();
    }

    public bool CheckHandisFull()
    {
        bool toReturn = false;
        if (allCards.Count >= 14)
        {
            toReturn = true;
        }
        return toReturn;
    }

    void SortCartes()
    {
        float startingXpos = ((allCards.Count - 1) / 2) * -70;
        startingXpos -= 419;
        for (int i = 0; i < allCards.Count; i++)
        {
            float gap = (-((allCards.Count - 1.0f) / 2.0f) + i);
            float angleToRotate = CalculGap(Mathf.Abs(gap));

            float y = angleToRotate * -4;
            y -= 480;
            allCards[i].go.GetComponent<RectTransform>().anchoredPosition = new Vector3(startingXpos + i * 70, y, 0);

            if (gap > 0)
            {
                angleToRotate = -angleToRotate;
            }
            allCards[i].go.transform.rotation = Quaternion.Euler(0, 0, 0);
            allCards[i].go.transform.Rotate(new Vector3(0, 0, angleToRotate));
            allCards[i].id = i;
            allCards[i].go.GetComponent<CartesButtons>().id = i;
        }
    }

    float CalculGap(float max)
    {
        float toReturn = max;
        for (int i = 0; i < max; i++)
        {
            toReturn += i;
        }
        return toReturn;
    }

    public void BaisserLesCartes()
    {
        for (int i = 0; i < cartesButtonsScripts.Length; i++)
        {
            cartesButtonsScripts[i].anim.SetBool("Baisser", true);
        }
    }

    public void ReleverLesCartes()
    {
        for (int i = 0; i < cartesButtonsScripts.Length; i++)
        {
            cartesButtonsScripts[i].anim.SetBool("Baisser", false);
        }
    }
    #endregion

    public void ChangeBoolPhases()
    {
        foreach (var go in allCards)
        {
            Destroy(go.go);
        }
        allCards.Clear();

        if (PhaseLente)
        {
            PhaseLente = false;
            ChangeBool.text = "Activer la phase lente.";
            scannerUI.gameObject.SetActive(false);
            scannerUI.gameObject.SetActive(true);
            scannerUI.GetComponent<Animator>().SetFloat("speedDraw", 1 / (drawTimer - 1));
            ennemiManager.PassageEnPhaseCombat();
        }
        else
        {
            ennemiManager.EndCombat();
            PhaseLente = true;
            ChangeBool.text = "Activer la phase combat.";
            scannerUI.gameObject.SetActive(false);
            ennemiManager.PassageEnPhaseLente();
        }
    }


    public void HandToModule(int index, ModuleManager mm)
    {
        var c = allCards[index];
        mm.cartesModule.Add(c);
        mm.MyModules[mm.cartesModule.Count - 1].GetComponent<Image>().sprite = mm.cartesModule[mm.cartesModule.Count - 1].illu;
        Destroy(allCards[index].go);
        allCards.RemoveAt(index);
    }

    public void ModuleToHand(ModuleManager mm)
    {
        if (mm.cartesModule.Count > 0)
        {
            print(mm.cartesModule.Count);
            var c = mm.cartesModule[mm.cartesModule.Count - 1];
            AjouterUneCarteDansLaMain(1, c.cartesTypes);
            mm.cartesModule.RemoveAt(mm.cartesModule.Count - 1);
            print(mm.cartesModule.Count);
        }

    }



}

