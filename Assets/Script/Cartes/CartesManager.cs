using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cartes
{
    public int id;
    public int cartesTypes;
    public Sprite illu;
    public Sprite picto;
    public GameObject go;
    public Cartes(int _id, int _cartesTypes)
    {
        id = _id;
        cartesTypes = _cartesTypes;
        illu = Resources.Load<Sprite>("Sprites/Cartes/V5/Carte" + _cartesTypes);
        picto = Resources.Load<Sprite>("Sprites/Cartes/Picto/Picto" + _cartesTypes);
    }
}

public class Salles
{
    public GameObject MyGo;
    public SetupManager SM;
    public bool CanPlayHere = true;
    
    public Salles(GameObject go)
    {
        MyGo = go;

        SM = MyGo.transform.GetChild(1).GetComponent<SetupManager>();
    }
}


public class CartesManager : MonoBehaviour
{
    [Header("Lists")]
    public List<Salles> allSalles = new List<Salles>();
    public List<Cartes> allCards = new List<Cartes>();
    CartesButtons[] cartesButtonsScripts = new CartesButtons[3];
    
    [Header("Other")]
    public GameObject prefabCarte;
    public Canvas canvas;
    public float drawTimer;
    public Image scannerUI;
    public Text ChangeBool;
    public GameObject SpawnEnnemiButton;
    public GameObject EnnemiHolder;
    
    public static bool PhaseLente = true;

    #region Initialisation
    void Awake()
    {
        scannerUI.GetComponent<Animator>().SetFloat("speedDraw", 1 / (drawTimer - 1));
    }

    void Start()
    {
        InitializeSalles();
        DrawCards();
        scannerUI.gameObject.SetActive(false);
    }

    void InitializeSalles()
    {
        for (int i = 0; i < 4; i++)
        {
            Salles newSalle = new Salles(GameObject.Find("Salle" + i.ToString()));
            allSalles.Add(newSalle);
        }
    }
    #endregion
    
    #region Actions
    public void PlayACardOnModule(int id, GameObject go)
    {
        int cardID = id;
        GameObject mGO = go;

        mGO.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = allCards[id].picto;
        
        var MM = mGO.transform.parent.GetComponent<ModuleManager>();
        for (int i = 0; i < MM.MyModules.Length; i++)
        {
            if (mGO == MM.MyModules[i].MyObject)
            {
                if (!PhaseLente || ((mGO == MM.MyModules[0].MyObject)||(mGO == MM.MyModules[1].MyObject)))
                {
                    MM.MyModules[i].CarteType = allCards[id].cartesTypes;
                    if (MM.MyModules[i].MyType == Modules.Type.Droite)
                    {
                        if (MM.MyModules[0].CarteType != -1 && MM.MyModules[1].CarteType != -1)
                        {
                            ThirdCardAction(MM);
                        }
                    }
                }
            }
        }

        if (!PhaseLente)
        {
            Destroy(allCards[cardID].go);
            allCards.Remove(allCards[cardID]);
            SortCartes();
        }
    }

    void ThirdCardAction(ModuleManager mm)
    {
        int type =  mm.MyModules[2].CarteType;
        mm.MyModules[2].CarteType = -1;
        mm.MyModules[2].MyObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Sprites/Cartes/Picto/cercleBlanc");

        
        for (int j = 0; j < allSalles.Count; j++)
        {
            if (mm.transform.parent.gameObject == allSalles[j].MyGo)
            {
                SelectSetup(type,mm);
            }
        }
    }

    public void SelectSetup(int type,ModuleManager mm)
    {
        int selectionOfSetup = 0;
        if (mm.MyModules[0].CarteType == 0)
        {
            //Attaque
            selectionOfSetup = 0;
        }
        else if (mm.MyModules[0].CarteType == 1)
        {
            //Def
            selectionOfSetup = 1;
        }
        else if (mm.MyModules[0].CarteType == 2)
        {
            //Alteration
            selectionOfSetup = 2; 
        }
        
        SelectCible(selectionOfSetup,type,mm);
    }

    public void SelectCible(int selectionOfSetup,int type,ModuleManager mm)
    {
        string wantedName;
        var calculTest = (Mathf.Abs(mm.MyModules[1].rotationCompteur) % 4);
        int wantedRoom = 0;
        if (Mathf.Abs(calculTest) == 0)
        {
            wantedRoom = 0;
        }
        else if (Mathf.Abs(calculTest) == 1)
        {
            wantedRoom = 1;
        }
        else if (Mathf.Abs(calculTest) == 2)
        {
            wantedRoom = 2;
        }
        else if (Mathf.Abs(calculTest) == 3)
        {
            wantedRoom = 3;
        }
        
        mm.MyCompteurInt -= 1;
        if (mm.MyCompteurInt <= 0)
        {
            mm.MyModules[1].CarteType = -1;
            mm.MyModules[1].MyObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>("Sprites/Cartes/Picto/cercleBlanc");
            mm.MyCompteurInt = 2;
        }
        mm.MyCompteur.text = mm.MyCompteurInt.ToString();
        
        
        if (selectionOfSetup == 0)
        {
            wantedName = mm.gameObject.transform.parent.transform.GetChild(1).GetComponent<SetupManager>().MySetupAttack.MyName;
        }
        else if (selectionOfSetup == 1)
        {
            wantedName = mm.gameObject.transform.parent.transform.GetChild(1).GetComponent<SetupManager>().MySetupDefense.MyName;
        }
        else
        {
            wantedName = mm.gameObject.transform.parent.transform.GetChild(1).GetComponent<SetupManager>().MySetupAlteration.MyName;
        }
        
        if (mm.MyModules[0].CarteType == type)
        {
            GetComponent<AllSetupsActions>().FindEffect(wantedName,wantedRoom);
        }
        else
        {
            GetComponent<AllSetupsActions>().FindEffect(wantedName,wantedRoom);
        }
    }
    #endregion
    
    #region Gestion Des Cartes
    public void DrawCards()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!CheckHandisFull())
            {
                AjouterUneCarteDansLaMain(1,i); 
            }
        }

        if (!PhaseLente)
        {
           StartCoroutine("Draw");
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
        if (PhaseLente)
        {
            ChangeBool.text = "Activer la phase lente.";
            scannerUI.gameObject.SetActive(false);
            scannerUI.gameObject.SetActive(true);
            scannerUI.GetComponent<Animator>().SetFloat("speedDraw", 1 / (drawTimer - 1));
            SpawnEnnemiButton.SetActive(true);
        }
        else
        {
            ChangeBool.text = "Activer la phase combat.";
            scannerUI.gameObject.SetActive(false);
            SpawnEnnemiButton.SetActive(false);
        }
        
        PhaseLente = !PhaseLente;
        foreach (var go in allCards)
        {
            Destroy(go.go);
        }
        allCards.Clear();
        DrawCards();
        
    }
}

