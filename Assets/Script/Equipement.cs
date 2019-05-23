using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Equipement : MonoBehaviour
{
    public class Equipements
    {
        public string action;
        public bool attaque;
        public int slot;
        public GameObject go;

        public Equipements()
        {

        }
    }

    public GameObject equipParent;
    public List<Equipements> allEquipements = new List<Equipements>();
    public string[] effets = new string[6];

    // Pour l'interface
    public Image imgAct;
    public Image imgCib;
    public Image imgOvr;
    public TextMeshProUGUI overdriveText;
    public Image ciblageSprite;
    
    public TextMeshProUGUI nomEquipement;
    public TextMeshProUGUI tags;
    public TextMeshProUGUI cdText;
    public TextMeshProUGUI equipementDescription;

    public TextMeshProUGUI nomDeLaSalle;
    

    private void Awake() {
        for (int i = 0; i < 6; i++)
        {
            Equipements a = new Equipements();
            a.go = equipParent.transform.GetChild(i).gameObject;
            a.action = a.go.name;
            a.slot = i;
            if (i == 2 || i == 4 || i == 5)
            {
                a.attaque = false;
            }
            else
            {
                a.attaque = true;
            }

            allEquipements.Add(a);
        }

        effets[4] = "<b><color=#4061BA>ELECTRONIC</color><b>" + "\n" + "Cleanse virus. Reducts virus duration of 8s";
        effets[5] = "<b><color=#6289F3>DURATION</color><b>" + "\n" + "Eliminates engine pressure and accelerates cooldown. Cooldown -30%. Last 8s.";
        effets[0] = "<b><color=#E6742E>PROJECTILE, CANALISATION</color></b>" + "\n" + "Fire a blast. Deals <color=#7841BB>35 damage</color>.";
        effets[3] = "<b><color=#E6742E>PROJECTILE, CANALISATION</color></b>" + "\n" + "Fire two blast. Each deals <color=#7841BB>27 damage</color>.";
        effets[1] = "<b><color=#B97D31>ELECTRONIC</color><b>" + "\n" + "Delays the execution of the planned action. Delay of 18s.";
        effets[2] = "<b><color=#6289F3>DURATION, ELECTRONIC</color><b>" + "\n" + "Prevent enemy targeting. Last 10s";
        
    }

        // "\n"
        // <color=#BC1910> normale
        // <color=#7841BB> intégrité
        // <color=#E6742E> attaque
        // <color=#4061BA> defense
        // <color=#6289F3> buff
        // <color=#B97D31> debuff
        // </color>

    public void AfficherTourelle()
    {
        
    }
}
