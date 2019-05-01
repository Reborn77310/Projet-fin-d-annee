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
    public SetupBaseSalle MySetup;
    
    public Salles(GameObject go, SetupBaseSalle setup)
    {
        MyGo = go;
        MySetup = setup;
    }
}

//Cercle = Defense 1 et Triangle = attaque 2
public enum SetupBaseSalle
{
    Cercle,Carre,Triangle  
}

public class CartesManager : MonoBehaviour
{
    [Header("Lists")]
    public List<Salles> allSalles = new List<Salles>();
    public List<SetupBaseSalle> LeSetupDesSalles = new List<SetupBaseSalle>();
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
            Salles newSalle = new Salles(GameObject.Find("Salle" + i.ToString()),LeSetupDesSalles[i]);
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
                            ThirdCardAction(MM,i);
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

    void ThirdCardAction(ModuleManager mm, int i)
    {
        int type =  mm.MyModules[i].CarteType;
        mm.MyModules[i].CarteType = -1;
        mm.MyModules[i].MyObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
            Resources.Load<Sprite>("Sprites/Cartes/Picto/cercleBlanc");

        
        for (int j = 0; j < allSalles.Count; j++)
        {
            if (mm.transform.parent.gameObject == allSalles[j].MyGo)
            {
                SelectSetup(allSalles[j].MySetup,mm.MyModules[1],type);
            }
        }
    }

    public void SelectSetup(SetupBaseSalle setup,Modules mod1,int type)
    {
        bool Attack = false;
        if (setup == SetupBaseSalle.Cercle)
        {
            //defense
            print("def");
        }
        else if (setup == SetupBaseSalle.Triangle)
        {
            //Attaque
            print("att");
            Attack = true;
        }
        SelectCible(Attack,mod1,type);
    }

    public void SelectCible(bool shouldIAttack,Modules mod1,int type)
    {
        if (shouldIAttack)
        {
            for (int i = 0; i < EnnemiManager.CurrentSpawns.Count; i++)
            {
                if (mod1.CarteType == 0)
                {
                    //Carré
                    //Cible 1-2
                  //  EnnemiManager.CurrentSpawns[i].MyChilds.myRooms[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/CercleBlanc");
                   // EnnemiManager.CurrentSpawns[i].MyChilds.myRooms[1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/CercleBlanc");
                    var typeSalle0 = EnnemiManager.CurrentSpawns[i].MyChilds.myTypes[0];
                    var typeSalle1 = EnnemiManager.CurrentSpawns[i].MyChilds.myTypes[1];
                    CalculDeDegats(type, typeSalle0,typeSalle1,i);
                }
                else if (mod1.CarteType == 1)
                {
                    //Cercle
                    //cible 1-3
                  //  EnnemiManager.CurrentSpawns[i].MyChilds.myRooms[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/CercleBlanc");
                  //  EnnemiManager.CurrentSpawns[i].MyChilds.myRooms[2].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/CercleBlanc");
                    var typeSalle0 = EnnemiManager.CurrentSpawns[i].MyChilds.myTypes[0];
                    var typeSalle2 = EnnemiManager.CurrentSpawns[i].MyChilds.myTypes[2];
                    CalculDeDegats(type, typeSalle0,typeSalle2,i);
                }
                else if (mod1.CarteType == 2)
                {
                    //Triangle
                    //Cible 1-4
                   // EnnemiManager.CurrentSpawns[i].MyChilds.myRooms[0].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/CercleBlanc");
                  //  EnnemiManager.CurrentSpawns[i].MyChilds.myRooms[3].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/CercleBlanc");
                    var typeSalle0 = EnnemiManager.CurrentSpawns[i].MyChilds.myTypes[0];
                    var typeSalle2 = EnnemiManager.CurrentSpawns[i].MyChilds.myTypes[3];
                    CalculDeDegats(type, typeSalle0,typeSalle2,i);
                }
            }
            EnnemiManager.CheckPdv();
        }
    }

    void CalculDeDegats(int myType, int theirType1, int theirType2,int i)
    {
        int degats = 0;
        if (myType == 0)
        {
            if (theirType1 == 0)
            {
                degats += 10;
            }
            else if (theirType1 == 1)
            {
                degats += 15;
            }
            else if (theirType1 == 2)
            {
                degats += 5;
            }
            if (theirType2 == 0)
            {
                degats += 10;
            }
            else if (theirType2 == 1)
            {
                degats += 15;
            }
            else if (theirType2 == 2)
            {
                degats += 5;
            }
           
        }
        if (myType == 1)
        {
            if (theirType1 == 0)
            {
                degats += 5;
            }
            else if (theirType1 == 1)
            {
                degats += 10;
            }
            else if (theirType1 == 2)
            {
                degats += 15;
            }
            if (theirType2 == 0)
            {
                degats += 5;
            }
            else if (theirType2 == 1)
            {
                degats += 10;
            }
            else if (theirType2 == 2)
            {
                degats += 15;
            }
           
        }
        if (myType == 2)
        {
            if (theirType1 == 0)
            {
                degats += 15;
            }
            else if (theirType1 == 1)
            {
                degats += 5;
            }
            else if (theirType1 == 2)
            {
                degats += 10;
            }
            if (theirType2 == 0)
            {
                degats += 15;
            }
            else if (theirType2 == 1)
            {
                degats += 5;
            }
            else if (theirType2 == 2)
            {
                degats += 10;
            }
        }
        EnnemiManager.PerdrePv(degats,i);
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

