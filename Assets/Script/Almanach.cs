using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class Almanach : MonoBehaviour
{
    public List<CartesManager.Cartes> cartesDebloquees = new List<CartesManager.Cartes>();
    public Image[] inventaire;
    public Image selectedCarteImage;
    public Image scType;
    public TextMeshProUGUI scRarity;
    public TextMeshProUGUI scDurability;
    public TextMeshProUGUI scTooltip;
    public int selectedCarteID;
    public int typeClic = 0;
    public int rarityClic = 0;
    public int optainedClic = 0;

    public void CreateCommunCards()
    {
        for (int i = 0; i < 5; i++)
        {
            CartesManager.Cartes c = new CartesManager.Cartes(i, i);
            c.rarity = 1;
            c.durability = c.rarity * 2;
            c.id = c.cartesTypes * 3 + c.rarity;
            c.overdriveEffect = c.id;
            c.illu = Resources.Load<Sprite>("Sprites/Cartes/Final/Cartes/Carte" + c.id);
            c.picto = Resources.Load<Sprite>("Sprites/Cartes/Final/picto" + c.cartesTypes);
            c.prefabZoneSelection = Resources.Load("Prefabs/Radar/ZoneSelection/Ciblage_" + c.id) as GameObject;
            c.optained = i;
            cartesDebloquees.Add(c);
        }
    }

    private void Awake()
    {
        CreateCommunCards();
    }

    public void DebloquerCarteRare()
    {
        CartesManager.Cartes c = new CartesManager.Cartes(cartesDebloquees.Count, 4);
        c.rarity = 2;
        c.durability = c.rarity * 2;
        c.id = c.cartesTypes * 3 + c.rarity;
        c.overdriveEffect = c.id;
        c.illu = Resources.Load<Sprite>("Sprites/Cartes/Final/Cartes/Carte" + c.id);
        c.picto = Resources.Load<Sprite>("Sprites/Cartes/Final/picto" + c.cartesTypes);
        c.prefabZoneSelection = Resources.Load("Prefabs/Radar/ZoneSelection/Ciblage_" + c.id) as GameObject;

        cartesDebloquees.Add(c);
    }

    public void DisplaySelectedCard()
    {
        string a = EventSystem.current.currentSelectedGameObject.name;
        a = a.Substring(2, 1); // appeler les emplacement bouton genre e_0, e_1, etc
        selectedCarteID = int.Parse(a);

        selectedCarteImage.sprite = cartesDebloquees[selectedCarteID].illu;
        scType.sprite = cartesDebloquees[selectedCarteID].picto;
        scDurability.text = cartesDebloquees[selectedCarteID].durability.ToString();
        //scTooltip.text = cartesDebloquees[selectedCarteID].overdriveEffect
        if (cartesDebloquees[selectedCarteID].rarity == 0)
        {
            scRarity.text = "<color=#B7B8BD><b>Common</b></color>";
        }
        else if (cartesDebloquees[selectedCarteID].rarity == 1)
        {
            scRarity.text = "<color=#475ED5><b>Rare</b></color>";
        }
        else if (cartesDebloquees[selectedCarteID].rarity == 2)
        {
            scRarity.text = "<color=#D5C447><b>Legendary</b></color>";
        }


    }

    public void AfficherInventaire()
    {
        for (int i = 0; i < inventaire.Length; i++)
        {
            if (i < cartesDebloquees.Count)
            {
                inventaire[i].sprite = cartesDebloquees[i].illu;
                // AJOUTER les TRUCS AUTOUR DES CARTES
            }
            else
            {
                // avoir si faut display un truc vide ou pas
            }
        }
    }

    public void TrierParTypes()
    {
        if (typeClic == 0)
        {
            cartesDebloquees.OrderBy(x => x.cartesTypes);
            typeClic = 1;
        }
        else if (typeClic == 1)
        {
            cartesDebloquees.OrderByDescending(x => x.cartesTypes);
            typeClic = 2;
        }
        else if (typeClic == 2)
        {
            cartesDebloquees.OrderBy(x => x.id);
            typeClic = 0;
        }

        AfficherInventaire();
    }

    public void TrierParRarete()
    {
        if (rarityClic == 0)
        {
            cartesDebloquees.OrderBy(x => x.rarity);
            rarityClic = 1;
        }
        else if (rarityClic == 1)
        {
            cartesDebloquees.OrderByDescending(x => x.rarity);
            rarityClic = 2;
        }
        else if (rarityClic == 2)
        {
            cartesDebloquees.OrderBy(x => x.id);
            rarityClic = 0;
        }
        AfficherInventaire();
    }

    public void TrierParLastOptain()
    {
        if (optainedClic == 0)
        {
            cartesDebloquees.OrderBy(x => x.optained);
            optainedClic = 1;
        }
        else if (optainedClic == 1)
        {
            cartesDebloquees.OrderByDescending(x => x.optained);
            optainedClic = 2;
        }
        else if (optainedClic == 2)
        {
            cartesDebloquees.OrderBy(x => x.id);
            optainedClic = 0;
        }
        AfficherInventaire();
    }

    public void textOverdrive(int index)
    {
        int i = cartesDebloquees[index].id;
        if (i == 0)
        {
            
        }
        else if (i == 1)
        {

        }
        else if (i == 2)
        {

        }
        else if (i == 3)
        {

        }
        else if (i == 4)
        {

        }
        else if (i == 5)
        {

        }
        else if (i == 6)
        {

        }
        else if (i == 7)
        {

        }
        else if (i == 8)
        {

        }
        else if (i == 9)
        {

        }
        else if (i == 10)
        {

        }
        else if (i == 11)
        {

        }
        else if (i == 12)
        {

        }
        else if (i == 13)
        {

        }
        else if (i == 14)
        {

        }
        else if (i == 15)
        {

        }
    }

}
