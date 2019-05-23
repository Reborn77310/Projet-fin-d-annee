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
            textOverdrive(i);
        }
    }

    private void Awake()
    {
        CreateCommunCards();
    }

    private void Start() {
        AfficherInventaire();
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
        textOverdrive(cartesDebloquees.Count - 1);
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
        int i = cartesDebloquees[index].id - 1;
        if (i == 0)
        {
            cartesDebloquees[index].textOverdrive = "Efficiency +30%" + "\n" + "<i>Boosts efficiency of equipment effect values ??by 30%.</i>";
        }
        else if (i == 1)
        {
            cartesDebloquees[index].textOverdrive = "Efficiency +60%" + "\n" + "<i>Boosts efficiency of equipment effect values ??by 60%.</i>";
        }
        else if (i == 2)
        {
            cartesDebloquees[index].textOverdrive = "Efficiency +90%" + "\n" + "<i>Boosts efficiency of equipment effect values ??by 90%.</i>";
        }
        else if (i == 3)
        {
            cartesDebloquees[index].textOverdrive = "Cooldown +/-30%" + "\n" + "<i>If the action targets a room of the N.E.S.T. it applies a buff that accelerates the cooldown of the room of 30%. In the other hand if the action targets an opponent's room it applies a debuff that slow the cooldown of the room of 30%. These effects last 6s.</i>";
        }
        else if (i == 4)
        {
            cartesDebloquees[index].textOverdrive = "Cooldown +/-60%" + "\n" + "<i>If the action targets a room of the N.E.S.T. it applies a buff that accelerates the cooldown of the room of 60%. In the other hand if the action targets an opponent's room it applies a debuff that slow the cooldown of the room of 60%. These effects last 8s.</i>";
        }
        else if (i == 5)
        {
            cartesDebloquees[index].textOverdrive = "Cooldown +/-90%" + "\n" + "<i>If the action targets a room of the N.E.S.T. it applies a buff that accelerates the cooldown of the room of 90%. In the other hand if the action targets an opponent's room it applies a debuff that slow the cooldown of the room of 90%. These effects last 10s.</i>";
        }
        else if (i == 6)
        {
            cartesDebloquees[index].textOverdrive = "Add <b>repetition</b> effect with an reduced efficiency of 50%" + "\n" + "<i>At the end of the action, it will be repeat a second time. The effect values of the second action will be reduced of 50%.</i>";
        }
        else if (i == 7)
        {
            cartesDebloquees[index].textOverdrive = "Add <b>repetition</b> effect with an reduced efficiency of 25%" + "\n" + "<i>At the end of the action, it will be repeat a second time. The effect values of the second action will be reduced of 25%.</i>";
        }
        else if (i == 8)
        {
            cartesDebloquees[index].textOverdrive = "Add <b>repetition</b> effect" + "\n" + "<i>At the end of the action, it will be repeat a second time.</i>";
        }
        else if (i == 9)
        {
            cartesDebloquees[index].textOverdrive = "Prolongs the <b>duration</b> of the action of 30%" + "\n" + "<i>If the action involves the tag </i>duration<i>, the duration of the effcts of the action will be prolonged of 30%.</i>";
        }
        else if (i == 10)
        {
            cartesDebloquees[index].textOverdrive = "Prolongs the <b>duration</b> of the action of 60%" + "\n" + "<i>If the action involves the tag </i>duration<i>, the duration of the effcts of the action will be prolonged of 60%.</i>";
        }
        else if (i == 11)
        {
            cartesDebloquees[index].textOverdrive = "Prolongs the <b>duration</b> of the action of 90%" + "\n" + "<i>If the action involves the tag </i>duration<i>, the duration of the effcts of the action will be prolonged of 90%.</i>";
        }
        else if (i == 12)
        {
            cartesDebloquees[index].textOverdrive = "Diffuse electronic effects around the targeted area" + "\n" + "<i>If the action involves the tag </i>electronic<i>, targeting area will be extended to adjacent squares. If rooms are hit will the extended area only the effects of the action will be apply with a reduced efficiency of 60%.</i>";
        }
        else if (i == 13)
        {
            cartesDebloquees[index].textOverdrive = "Diffuse electronic effects around the targeted area" + "\n" + "<i>If the action involves the tag </i>electronic<i>, targeting area will be extended to adjacent squares. If rooms are hit will the extended area only the effects of the action will be apply with a reduced efficiency of 40%.</i>";
        }
        else if (i == 14)
        {
            cartesDebloquees[index].textOverdrive = "Diffuse electronic effects around the targeted area" + "\n" + "<i>If the action involves the tag </i>electronic<i>, targeting area will be extended to adjacent squares. If rooms are hit will the extended area only the effects of the action will be apply with a reduced efficiency of 20%.</i>";
        }
    }

}
