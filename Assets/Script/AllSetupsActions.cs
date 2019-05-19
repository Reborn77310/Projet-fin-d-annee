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
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Attack
        if (wantedName == "Tourelle BK-1") // ATK
        {
            string effet = "Tire une rafale";
            DealDamage(wantedRoom, 35,11);
            print(effet);
        }
        else if (wantedName == "Tourelle BK-2") // ATK
        {
            string effet = "Tire deux rafales";
            print(effet);
        }
        else if (wantedName == "Bombe IEM") // ATK
        {
            string effet = "Retarde l’exécution de la prochaine action";
            print(effet);
        }
        else if (wantedName == "Brouilleur") // DEF
        {
            string effet = "Empêche le ciblage ennemi";
            print(effet);
        }
        else if (wantedName == "Turbine") // DEF
        {
            string effet = "Evacue la pression des moteurs et accélère le temps de recharge";
            print(effet);
        }
        else if (wantedName == "hgOS") // DEF
        {
            string effet = "Programme qui purge des virus";
            // salleManager.allSalles[wantedRoom].isDefendu = true;
            // salleManager.DefendreSalle(wantedRoom);
            // StartCoroutine(instance.BlindeMaximalAndCloak(4.0f,wantedRoom));
            print(effet);
        }
    }

    IEnumerator BlindeMaximalAndCloak(float timer,int i)
    {
        yield return new WaitForSeconds(timer);
        salleManager.allSalles[i].isDefendu = false;
        salleManager.CancelDefense(i);
        yield break;
    }
    
    public void DealDamage(int wantedRoom,int damage,float cooldown)
    {
        ennemiManager.PerdrePvGlobal(damage);
        ennemiManager.PerdrePvLocal(wantedRoom,damage);
        ennemiManager.CheckPdv();
           
        salleManager.MakeCooldownSalle(SalleQuiEffectueAction,cooldown);
    }
}
