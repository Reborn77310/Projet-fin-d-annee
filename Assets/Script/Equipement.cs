using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        effets[4] = "ELECTRONIC. Cleanse virus. Reducts virus duration of 8s";
        effets[5] = "DURATION. Eliminates engine pressure and accelerates cooldown. Cooldown -30%. Last 8s.";
        effets[0] = "CANALISATION. Fire a blast. Deals 35 damage";
        effets[3] = "CANALISATION. Fire two blast. Each deals 27 damage";
        effets[1] = "CHARGE. ELECTRONIC. Delays the execution of the next action. Delay of 8s.";
        effets[2] = "DURATION. ELECTRONIC. Prevent enemy targeting. Last 10s";
        
    }


}
