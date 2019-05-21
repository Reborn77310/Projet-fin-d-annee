using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Almanach : MonoBehaviour
{
    public List<CartesManager.Cartes> cartesDebloquees = new List<CartesManager.Cartes>();

    //     public int id; // L'index dans la liste à laquelle elle appartient (allCards ou onModule)
    //     public int cartesTypes; // 0,1,2 = types basiques // 10+ = types spéciaux
    //     public Sprite illu;
    //     public Sprite picto;
    //     public GameObject go;

    //     // A SETUP :
    //     public int durability;
    //     public GameObject prefabZoneSelection;
    //     public int rarity;
    //     public int overdriveEffect;
    
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

            cartesDebloquees.Add(c);
        }
    }

    private void Awake() {
        CreateCommunCards();
    }
}
