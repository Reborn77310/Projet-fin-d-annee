using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public void FindEffect(string wantedName, int wantedRoom, ModuleManager mm, bool superEffect)
    {
        SalleQuiEffectueAction = mm.MySalleNumber;
        float cooldown = 0;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Attack
        if (wantedName == "Tourelle BK-1") // ATK
        {
            string effet = "Tire une rafale";
            if (ennemiManager.ennemiRooms[wantedRoom].reflect)
            {
                salleManager.DamageSurSalle(mm.MySalleNumber, 35);
            }
            float damage = 35;
            damage *= ennemiManager.ennemiRooms[wantedRoom].projectileReduction;
            damage *= ennemiManager.ennemiRooms[wantedRoom].cannalisationReduction;
            DealDamage(wantedRoom, damage);
            cooldown = 11;
            print(effet);
        }
        else if (wantedName == "Tourelle BK-2") // ATK
        {
            string effet = "Tire deux rafales";
            if (ennemiManager.ennemiRooms[wantedRoom].reflect)
            {
                salleManager.DamageSurSalle(mm.MySalleNumber, 54);
            }
            float damage = 27;
            damage *= ennemiManager.ennemiRooms[wantedRoom].projectileReduction;
            damage *= ennemiManager.ennemiRooms[wantedRoom].cannalisationReduction;
            DealDamage(wantedRoom, damage);
            DealDamage(wantedRoom, damage);
            cooldown = 14;
            print(effet);
        }
        else if (wantedName == "CanonIEM") // ATK
        {
            if (ennemiManager.ennemiRooms[wantedRoom].reflect)
            {
                string[] a = new string[] { "DURATION"};
                salleManager.AddEffets(4, "Tempo", a, mm.MySalleNumber, -0.5f);
            }
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
            int x = 0;
            for(int i = 0; i < salleManager.allSalles.Count; i++)
            {
                if(salleManager.allSalles[i].canBeTarget)
                x++;
            }
            if (x > 1)
            {
                salleManager.allSalles[wantedRoom].canBeTarget = false;
            }
            
            string[] a = new string[] { "DURATION", "ELECTRONIC" };
            salleManager.AddEffets(10, "Brouilleur", a, wantedRoom, 0);
            cooldown = 22;
            string effet = "Empêche le ciblage ennemi";
            print(effet);
        }
        else if (wantedName == "Turbine") // DEF
        {
            ModuleManager moduleManager = salleManager.allSalles[wantedRoom].MyGo.GetComponent<ModuleManager>();
            if (moduleManager.cartesModule.Count > 1)
            {
                moduleManager.cartesModule[1].durability += 1;
            }
            string[] a = new string[] { "DURATION" };
            salleManager.AddEffets(6, "Tempo", a, wantedRoom, 0.3f);
            cooldown = 11;
            string effet = "Evacue la pression des moteurs et accélère le temps de recharge";
            print(effet);
        }
        else if (wantedName == "hgOS") // DEF
        {
            // check si wanted room a des effets electronic
            if (salleManager.allEffets.Exists(item => item.salle == wantedRoom && item.tags.Contains("ELECTRONIC")))
            {
                var temp = salleManager.allEffets.FindAll(item => item.salle == wantedRoom && item.tags.Contains("ELECTRONIC"));
                for (int i = 0; i < temp.Count; i++)
                {
                    temp[i].duration -= 8;
                }
            }
            cooldown = 24;
            string effet = "Programme qui purge des virus";
            print(effet);
        }
        salleManager.EnterCooldown(SalleQuiEffectueAction, cooldown);
    }

    public void DealDamage(int wantedRoom, float damage)
    {
        ennemiManager.PerdrePvGlobal(damage);
        ennemiManager.PerdrePvLocal(wantedRoom, damage);
        ennemiManager.CheckPdv();
    }

}
