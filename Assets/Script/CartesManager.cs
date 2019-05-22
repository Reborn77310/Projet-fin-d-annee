using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;




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
        }
    }
    [Header("Lists")]
    public List<Cartes> allCards = new List<Cartes>();
    public List<Cartes> bufferDraw = new List<Cartes>();
    CartesButtons[] cartesButtonsScripts = new CartesButtons[3];

    [Header("Other")]
    public GameObject prefabCarte;
    public GameMaster gameMaster;
    public Canvas canvas;
    float drawTimer = 30;
    public Text ChangeBool;
    public EnnemiManager ennemiManager;
    public Image grilleRadar;
    public static bool PhaseLente = true;
    public Image versus;
    public GameObject cartesHolder;
    Almanach almanach;

    #region Initialisation
    void Awake()
    {
        almanach = GetComponent<Almanach>();
    }

    void Start()
    {
        DrawCards();
    }


    #endregion

    #region Actions
    public void PlayACardOnModule(int id, ModuleManager mm)
    {
        if (!PhaseLente)
        {
            gameMaster.cardSound.GoingToPlayACard();
            gameMaster.consolesAnim[mm.MySalleNumber].SetTrigger("validation");
            HandToModule(id, mm);
            if (mm.cartesModule.Count == 3)
            {
                ThirdCardAction(mm);
            }
            else
            {
                StartCoroutine("StopAnim", mm);
            }
            SortCartes();
        }
        else
        {

            if (mm.cartesModule.Count < 2)
            {
                gameMaster.cardSound.GoingToPlayACard();
                var c = allCards[id];
                mm.cartesModule.Add(c);
                mm.slotImage[mm.cartesModule.Count - 1].sprite = mm.cartesModule[mm.cartesModule.Count - 1].picto;
                SortCartes();
                StartCoroutine("StopAnim", mm);
            }

        }
    }

    IEnumerator StopAnim(ModuleManager mm)
    {
        gameMaster.consolesAnim[mm.MySalleNumber].SetTrigger("validation");
        yield return new WaitForSeconds(1f);
        gameMaster.consolesAnim[mm.MySalleNumber].SetBool("hover", false);
    }

    void ThirdCardAction(ModuleManager mm)
    {
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
                    AjouterUneCarteDansLaMain(toDraw[i]);
                }
            }
            StartCoroutine("Draw");
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                if (!CheckHandisFull())
                {
                    AjouterUneCarteDansLaMain(i % 3);
                }
            }
        }
    }

    IEnumerator Draw()
    {
        float timer = 0;
        while (timer < drawTimer)
        {
            timer += Time.deltaTime;
            versus.GetComponent<Image>().fillAmount = timer / drawTimer;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        DrawCards();
        yield break;
    }

    public void AjouterUneCarteDansLaMain(int carteType)
    {
        GameObject a = Instantiate(prefabCarte, Vector3.zero, Quaternion.identity, cartesHolder.transform);
        Cartes b = CardFabrication(carteType);
        Cartes c = new Cartes(allCards.Count, carteType);
        CopyCardsValue(b, c);
        c.go = a;
        CartesButtons cb = a.GetComponent<CartesButtons>();
        cb.id = c.id;
        a.GetComponent<Image>().sprite = c.illu;
        allCards.Add(c);
        //Debug.Log(c.id + " " + allCards.IndexOf(c));

        SortCartes();
    }

    public Cartes CardFabrication(int cartesTypes)
    {
        bufferDraw.Clear();
        bufferDraw = almanach.cartesDebloquees.FindAll(item => item.cartesTypes.Equals(cartesTypes));
        if (bufferDraw.Count == 1)
        {
            return bufferDraw[0];
        }
        else if (bufferDraw.Count == 2)
        {
            float rnd = Random.value * 100;
            if (rnd <= 60)
            {
                return bufferDraw[0];
            }
            else
            {
                return bufferDraw[1];
            }
        }
        else
        {
            float rnd = Random.value * 100;
            if (rnd <= 50)
            {
                return bufferDraw[0];
            }
            else if (rnd > 50 && rnd <= 85)
            {
                return bufferDraw[1];
            }
            else
            {
                return bufferDraw[2];
            }
        }
    }

    void CopyCardsValue(Cartes oldC, Cartes newC)
    {
        newC.rarity = oldC.rarity;
        newC.durability = oldC.durability;
        newC.overdriveEffect = oldC.overdriveEffect;
        newC.illu = oldC.illu;
        newC.picto = oldC.picto;
        newC.prefabZoneSelection = oldC.prefabZoneSelection;
    }


    public bool CheckHandisFull()
    {
        bool toReturn = false;
        if (allCards.Count >= 6)
        {
            toReturn = true;
        }
        return toReturn;
    }

    void SortCartes()
    {
        float startingXpos = ((allCards.Count - 1) / 2) * -120;
        startingXpos -= 460; // anciennement 419
        for (int i = 0; i < allCards.Count; i++)
        {
            float gap = (-((allCards.Count - 1.0f) / 2.0f) + i);
            float angleToRotate = CalculGap(Mathf.Abs(gap));

            float y = angleToRotate * -4; // -4
            y -= 480; // 480
            allCards[i].go.GetComponent<RectTransform>().anchoredPosition = new Vector3(startingXpos + i * 120, y, 0);

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

    public void SortCartesOnPointerEnter(int id)
    {
        allCards[id].go.transform.SetAsLastSibling();

        float startingXpos = ((allCards.Count - 1) / 2) * -120;
        startingXpos -= 460; // anciennement 419

        for (int i = 0; i < allCards.Count; i++)
        {
            int dif = i - id;
            float toGive = startingXpos + i * 120;
            if (Mathf.Abs(dif) <= 3 && dif != 0)
            {
                float gap = (-((allCards.Count - 1.0f) / 2.0f) + i);
                float angleToRotate = CalculGap(Mathf.Abs(gap));
                float y = angleToRotate * -4; // -4
                y -= 480; // 480
                allCards[i].go.GetComponent<DecalageCartes>().EcarterCarte(toGive + 110 / dif, y);
            }
            else if (dif == 0)
            {
                allCards[id].go.GetComponent<DecalageCartes>().Highlight(toGive);
            }

        }
        allCards[id].go.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void TuLuiDonneSonIndex(int id)
    {
        float startingXpos = ((allCards.Count - 1) / 2) * -120;
        startingXpos -= 460; // anciennement 419
        for (int i = 0; i < allCards.Count; i++)
        {
            float gap = (-((allCards.Count - 1.0f) / 2.0f) + i);
            float angleToRotate = CalculGap(Mathf.Abs(gap));
            if (i != id)
            {
                float y = angleToRotate * -4; // -4
                y -= 480; // 480
                float toGive = startingXpos + i * 120;
                allCards[i].go.GetComponent<DecalageCartes>().RetourNormale(toGive, y);
            }
            else
            {
                float toGive = startingXpos + i * 120;
                float y = angleToRotate * -4; // -4
                y -= 480; // 480
                allCards[id].go.GetComponent<DecalageCartes>().RetourHighlight(toGive, y);
            }

            if (gap > 0)
            {
                angleToRotate = -angleToRotate;
            }
            allCards[i].go.transform.rotation = Quaternion.Euler(0, 0, 0);
            allCards[i].go.transform.Rotate(new Vector3(0, 0, angleToRotate));

            allCards[i].go.transform.SetSiblingIndex(allCards[i].id);
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
            ennemiManager.boutonSpawn.SetActive(true);
        }
        else
        {
            ennemiManager.EndCombat();
            PhaseLente = true;
            ChangeBool.text = "Activer la phase combat.";
            ennemiManager.PassageEnPhaseLente();
        }
    }


    public void HandToModule(int index, ModuleManager mm)
    {
        var c = allCards[index];
        mm.cartesModule.Add(c);
        mm.slotImage[mm.cartesModule.Count - 1].sprite = mm.cartesModule[mm.cartesModule.Count - 1].picto;
        Destroy(allCards[index].go);
        allCards.RemoveAt(index);
    }

    public void ModuleToHand(ModuleManager mm)
    {
        if (mm.cartesModule.Count > 0)
        {
            var c = mm.cartesModule[mm.cartesModule.Count - 1];
            AjouterUneCarteDansLaMain(c.cartesTypes);
            mm.cartesModule.RemoveAt(mm.cartesModule.Count - 1);
        }
    }

}

