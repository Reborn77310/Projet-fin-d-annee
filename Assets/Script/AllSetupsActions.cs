﻿using System.Collections;
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
        if (wantedName == "Attaque 01")
        {
            string effet = "Inflige des dégâts sur la salle ciblée";
            DealDamage(wantedRoom, 20,5);
            // 10 % * 2
            // 15 % * 2
            print(effet);
        }
        else if (wantedName == "Attaque 02")
        {
            string effet = "Inflige des dégâts sur la salle ciblée";
            // 10% 
            // 15% proc attaque
            DealDamage(wantedRoom, 10,5);
            print(effet);
        }
        else if (wantedName == "Attaque 03")
        {
            string effet = "Tir des projectiles sur des cibles aléatoire (peut toucher plusieurs fois la même salle)";
            //3 proj à 10% aléa
            //4proj
            // DealDamage(Random.Range(0, 4),10);
            // DealDamage(Random.Range(0, 4),10);
            // DealDamage(Random.Range(0, 4),10);
            if (superEffect)
            {
                DealDamage(wantedRoom, 75,5);
                print(effet + " super ++");
            }
            else
            {
                DealDamage(wantedRoom, 35,5);
                print(effet);
            }
            
        }
        else if (wantedName == "Attaque 04")
        {
            string effet = "Si 2 symboles différents ont été posé et burn précedemment, le troisième symbole déclenche un tir puissant.";
            //Jouer les trois cartes dans le slot 3
            print(effet);
        }
        else if (wantedName == "Attaque 05")
        {
            string effet = "Tir un projectile, inflige des dégâts sur la salle ciblé.";
            //20 %
            //Augmente 10% 
            DealDamage(wantedRoom, 20,5);
            print(effet);
        }
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Defense
        else if (wantedName == "Blindage maximal")
        {
            string effet = "Rend la salle visée insensible aux dégâts et aux altérations";
            salleManager.allSalles[wantedRoom].isDefendu = true;
            salleManager.DefendreSalle(wantedRoom);
            StartCoroutine(instance.BlindeMaximalAndCloak(4.0f,wantedRoom));
            print(effet);
        }
        else if (wantedName == "Cape d'invisibilité")
        {
            string effet = "Empêche l'ennemi de cibler la salle visée et d'avoir n'importe quelle info dessus";
            salleManager.allSalles[wantedRoom].isDefendu = true;
            if (superEffect)
            {
                salleManager.allSalles[wantedRoom].DefendingAmount = 100;
            }
            else
            {
                salleManager.allSalles[wantedRoom].DefendingAmount = 50;
            }
            
            print(salleManager.allSalles[wantedRoom].DefendingAmount);
            salleManager.DefendreSalle(wantedRoom);
            StartCoroutine(instance.BlindeMaximalAndCloak(5.0f,wantedRoom));
            print(effet);
        }
        else if (wantedName == "Defense 03")
        {
            string effet = "Place un bouclier sur la salle visée qui bloque 50% des dégâts reçu lors de la prochaine attaque";
            print(effet);
        }
        else if (wantedName == "Defense 04")
        {
            string effet = "Place un bouclier de 5 secondes sur toutes les salles qui réduit de 15% les dégâts subit";
            print(effet);
        }
        
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Alteration
        else if (wantedName == "Impulsion IEM")
        {
            string effet = "Immobilise la salle ciblée pendant une action";
            print(effet);
        }
        else if (wantedName == "Drone reco")
        {
            string effet = "Permet d'avoir des informations précise sur les actions de la salle adverse visée";
            print(effet);
        }
        else if (wantedName == "Image mirroir")
        {
            string effet = "Fait croire au système ennemi que deux 2 ennemis supplémentaire sont apparus. L'ennemi choisit alors aléatoirement la cible";
            print(effet);
        }
        else if (wantedName == "Confusion")
        {
            string effet = "Prend ses alliées pour des ennemis, les attaques pendant 2 actions";
            print(effet);
        }
        else if (wantedName == "Corruption")
        {
            string effet = "Prend le controle de la salle ciblée. Cette salle s'adapte aux mêmes setup et jouera les mêmes cartes pendant 2 actions";
            print(effet);
        }
        else if (wantedName == "Debuf")
        {
            string effet = "La salle visée subira 50% de dégâts en plus lors de la prochaine attaque";
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
