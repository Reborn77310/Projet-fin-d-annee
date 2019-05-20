using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSetupsActions : MonoBehaviour
{
    public static AllSetupsActions instance;
    private SalleManager salleManager;
    public EnnemiManager ennemiManager;
    private int SalleQuiEffectueAction = 0;
    
    void Awake()
    {
        instance = this;
        salleManager = GameObject.Find("GameMaster").GetComponent<SalleManager>();
    }
    
    public void FindEffect(string wantedName, int wantedRoom, ModuleManager mm,bool superEffect)
    {
        SalleQuiEffectueAction = mm.MySalleNumber;
        float cooldown = 0;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Attack
        if (wantedName == "Tourelle BK-1") // ATK
        {
            string effet = "Tire une rafale";
            DealDamage(wantedRoom, 35);
            cooldown = 11;
            print(effet);
        }
        else if (wantedName == "Tourelle BK-2") // ATK
        {
            string effet = "Tire deux rafales";
            DealDamage(wantedRoom, 27);
            DealDamage(wantedRoom, 27);
            cooldown = 14;
            print(effet);
        }
        else if (wantedName == "CanonIEM") // ATK
        {
            
            if (ennemiManager.actionPrevues.Exists(item => item.origine == wantedRoom))
            {
                int test = ennemiManager.actionPrevues.Find(item => item.origine == wantedRoom).origine;
                ennemiManager.actionPrevues[test].timer += 18;
                ennemiManager.ennemiRooms[wantedRoom].timer += 18;
            }
            else
            {
                // Miss
            }
            cooldown = 14;
            string effet = "Retarde l’exécution de la prochaine action";
            print(effet);
        }
        else if (wantedName == "Brouilleur") // DEF
        {
            cooldown = 22;
            string effet = "Empêche le ciblage ennemi";
            print(effet);
        }
        else if (wantedName == "Turbine") // DEF
        {
            cooldown = 11;
            string effet = "Evacue la pression des moteurs et accélère le temps de recharge";
            print(effet);
        }
        else if (wantedName == "hgOS") // DEF
        {
            cooldown = 14;
            string effet = "Programme qui purge des virus";
            // salleManager.allSalles[wantedRoom].isDefendu = true;
            // salleManager.DefendreSalle(wantedRoom);
            // StartCoroutine(instance.BlindeMaximalAndCloak(4.0f,wantedRoom));
            print(effet);
        }
        salleManager.EnterCooldown(SalleQuiEffectueAction,cooldown);
    }

    public void DealDamage(int wantedRoom,int damage)
    {
        ennemiManager.PerdrePvGlobal(damage);
        ennemiManager.PerdrePvLocal(wantedRoom,damage);
        ennemiManager.CheckPdv();        
    }
}
