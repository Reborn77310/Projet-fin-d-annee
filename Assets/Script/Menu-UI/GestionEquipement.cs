using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Rooms
{
    private int RoomNumber;
    public Sprite[] Modules = new Sprite[3];
    public Sprite Ciblage;
    public string EquipementName;
    public string Type;
    public string CD;
    public string Description;
    public string Overdrive;
    
    public int CarteTypeModule1 = -1;
    
    public Rooms(int roomNumber)
    {
        RoomNumber = roomNumber;
    }
}

public class GestionEquipement : MonoBehaviour
{
    GestionBoutonUi gbi;
    private SalleManager sm;
    
    private int actualRoom = 0;
    int whoOppenedCards = 0;
    
    private Sprite[] spritesCiblages;
    public GameObject CarteSelection;
    public Image Ciblage;
    
    public Rooms[] myRooms = new Rooms[4];
    
    public Image[] Module = new Image[3];
    public TextMeshProUGUI Header;
    public TextMeshProUGUI EquipementName;
    public TextMeshProUGUI EquipementType;
    public TextMeshProUGUI EquipementCD;
    public TextMeshProUGUI EquipementDescription;
    public TextMeshProUGUI OverdriveDescription;
    
    void Awake()
    {
        sm = GetComponent<SalleManager>();
        gbi = GameObject.Find("GameMaster").GetComponent<GestionBoutonUi>();
        
        Sprite blank = Resources.Load<Sprite>("Sprites/Cartes/CarteBlank") as Sprite;
        spritesCiblages = Resources.LoadAll<Sprite>("Sprites/Cartes/Ciblages");
        
        for (int i = 0; i < 4; i++)
        {
            myRooms[i] = new Rooms(i);
            for (int j = 0; j < 3; j++)
            {
                myRooms[i].Modules[j] = blank;
            }

            myRooms[i].EquipementName = "";
            myRooms[i].Type = "";
            myRooms[i].CD = "";
            myRooms[i].Description = "";
            myRooms[i].Overdrive = "";
            myRooms[i].Ciblage = blank;
        }

        ActualiseModule();
        ActualiseText();
    }
    
    public void ChangeRoom(int wantedRoom)
    {
        Header.text = "<color=#78FF1B>Room </color>" + (wantedRoom + 1);
        gbi.ActualRoom = wantedRoom;
        actualRoom = wantedRoom;
        ActualiseModule();
        ActualiseText();
    }

    public void ClickedOnModule(GameObject moduleWanted)
    {
        int ButtonNumber = int.Parse(moduleWanted.name);
        
        if (!CarteSelection.activeInHierarchy)
        {
            CarteSelection.SetActive(true);
        }
        else if(CarteSelection.activeInHierarchy && whoOppenedCards == ButtonNumber)
        {
            CarteSelection.SetActive(false);
        }
        
        whoOppenedCards = ButtonNumber;
    }

    public void SelectionDeLaCarte(GameObject thisCard)
    {
        int ButtonNumber = int.Parse(thisCard.name);
        Sprite wantedSprite = Resources.Load<Sprite>("Sprites/Cartes/Final/Cartes/Carte" + ButtonNumber) as Sprite;
        if (whoOppenedCards == 0)
        {
            myRooms[actualRoom].Modules[0] = wantedSprite;
            myRooms[actualRoom].Modules[2] = wantedSprite;
            if (ButtonNumber >= 10)
            {
                myRooms[actualRoom].CarteTypeModule1 = 1;
            }
            else
            {
                myRooms[actualRoom].CarteTypeModule1 = 0;
            }
           FindInfosFirstModule();
           FindInfoOverdrive(ButtonNumber);
        }
        else if (whoOppenedCards == 1)
        {
            myRooms[actualRoom].Modules[1] = wantedSprite;
            ActualiseCiblageWithModule2(ButtonNumber);
        }
        
        ActualiseModule();
        CarteSelection.SetActive(false);
    }

    void ActualiseModule()
    {
        for (int i = 0; i < 3; i++)
        {
            Module[i].sprite = myRooms[actualRoom].Modules[i];
        }
        
        Ciblage.sprite = myRooms[actualRoom].Ciblage;
        
    }

    void ActualiseText()
    {
        EquipementName.text = "<color=#1E8811>" + myRooms[actualRoom].EquipementName + " </color>";
        EquipementType.text = "<color=#E6742E>" + myRooms[actualRoom].Type + " </color>";
        EquipementCD.text = "<color=#1C820D>" + myRooms[actualRoom].CD + " </color>";
        EquipementDescription.text = "<color=#1C820D>" + myRooms[actualRoom].Description + " </color>";
    }
    void ActualiseTextOverdrive()
    {
        OverdriveDescription.text = "<color=#1D8210>" + myRooms[actualRoom].Overdrive + " </color>";
    }
    void ActualiseCiblageWithModule2(int index)
    {
        Ciblage.sprite = spritesCiblages[index-1];
        myRooms[actualRoom].Ciblage = Ciblage.sprite;
    }

    void FindInfosFirstModule()
    {
        if ((actualRoom == 0 && myRooms[actualRoom].CarteTypeModule1 == 1) ||(actualRoom == 3 && myRooms[actualRoom].CarteTypeModule1 == 1)) // CANON IEM
        {
            myRooms[actualRoom].EquipementName = "CanonIEM";
            myRooms[actualRoom].Type = "CHARGE, ELECTRONIQUE";
            myRooms[actualRoom].CD = "14 secondes";
            myRooms[actualRoom].Description = "Retarde l’execution de la prochaine action. Retard de 4s";
        }
        else if ((actualRoom == 1 && myRooms[actualRoom].CarteTypeModule1 == 1) ||(actualRoom == 2 && myRooms[actualRoom].CarteTypeModule1 == 1)) //hg0S
        {
            myRooms[actualRoom].EquipementName = "hg0s";
            myRooms[actualRoom].Type = "Durée";
            myRooms[actualRoom].CD = "24 secondes";
            myRooms[actualRoom].Description = "Programme qui purge les virus. Réduit le temps de présence de 8 secondes.";
        }
        else if (actualRoom == 0 && myRooms[actualRoom].CarteTypeModule1 == 0) //Tourelle BK1
        {
            myRooms[actualRoom].EquipementName = "Tourelle BK-1";
            myRooms[actualRoom].Type = "PROJECTILE, CANALISATION";
            myRooms[actualRoom].CD = "11 secondes";
            myRooms[actualRoom].Description = "Tire une rafale qui inflige 35 dégâts";
        }
        else if (actualRoom == 1 && myRooms[actualRoom].CarteTypeModule1 == 0) //Tourelle BK2
        {
            myRooms[actualRoom].EquipementName = "Tourelle BK-2";
            myRooms[actualRoom].Type = "PROJECTILE, CANALISATION";
            myRooms[actualRoom].CD = "14 secondes";
            myRooms[actualRoom].Description = "Tire deux rafales qui infligent 27 dégâts chacunes";
        }
        else if (actualRoom == 2 && myRooms[actualRoom].CarteTypeModule1 == 0) //Turbine
        {
            myRooms[actualRoom].EquipementName = "Turbines à vapeur";
            myRooms[actualRoom].Type = "DURÉE";
            myRooms[actualRoom].CD = "11 secondes";
            myRooms[actualRoom].Description = "Évacue la pression des moteurs et accélère le temps de recharge. Recharge -30%. Dure 6 secondes.";
        }
        else if (actualRoom == 3 && myRooms[actualRoom].CarteTypeModule1 == 0) //Brouilleur
        {
            myRooms[actualRoom].EquipementName = "Brouilleur";
            myRooms[actualRoom].Type = "DURÉE, ELECTRONIQUE";
            myRooms[actualRoom].CD = "22 secondes";
            myRooms[actualRoom].Description = "Empêche le ciblage ennemi. Dure 10 secondes.";
        }
        ActualiseText();
    }

    void FindInfoOverdrive(int numeroCarte)
    {
        if (numeroCarte >= 1 && numeroCarte < 4) // Carte bleue
        {
            myRooms[actualRoom].Overdrive = "Renforce l'efficacité de l'équipement sélectionné de 30%.";
        }
        else if (numeroCarte >= 4 && numeroCarte < 7) // Carte verte
        {
            myRooms[actualRoom].Overdrive = "Renforce l'efficacité de l'équipement sélectionné de 30%.";
        }
        else if (numeroCarte >= 7 && numeroCarte < 10) // Carte jaune
        {
            myRooms[actualRoom].Overdrive = "L'effet de l'équipement se joue deux fois, la deuxième fois avec une efficacité réduite de 50%."; 
        }
        else if (numeroCarte >= 10 && numeroCarte < 12) // Carte rose
        {
            myRooms[actualRoom].Overdrive = "Overdrive non integré."; 
        }
        else if (numeroCarte >= 13 && numeroCarte < 15) // Carte rouge
        {
            myRooms[actualRoom].Overdrive = "Overdrive non integré."; 
        }
        ActualiseTextOverdrive();
    }

    
}
