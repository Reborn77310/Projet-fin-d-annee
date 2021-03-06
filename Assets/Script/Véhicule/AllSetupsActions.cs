﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AllSetupsActions : MonoBehaviour
{
    public static AllSetupsActions instance;
    private SalleManager salleManager;
    public EnnemiManager ennemiManager;
    private int SalleQuiEffectueAction = 0;
    public Animator[] MyAnimatorsTurrets = new Animator[6];

    public float firstActionTimer = 0.0f;
    public float secondActionTimer = 0.0f;

    void Awake()
    {
        instance = this;
        salleManager = GameObject.Find("GameMaster").GetComponent<SalleManager>();
    }

    public void FindEffect(string wantedName, int wantedRoom, ModuleManager mm, bool superEffect, int equipementSelected)
    {
        if (equipementSelected >= 0)
        {
            MyAnimatorsTurrets[equipementSelected].SetTrigger("Declenche");
        }

        SalleQuiEffectueAction = mm.MySalleNumber;

        if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
        {
            if (!XML_PlaytestAnalyse.firstFight)
            {
                XML_PlaytestAnalyse.NombreDeFoisCarteUtiliseeSurUnModuleUnPremierCombat[mm.cartesModule[0].cartesTypes] += 1;
                XML_PlaytestAnalyse.NombreDeFoisCarteUtiliseeSurUnModuleDeuxPremierCombat[mm.cartesModule[1].cartesTypes] += 1;
                XML_PlaytestAnalyse.NombreDeFoisCarteUtiliseeSurUnModuleTroisPremierCombat[mm.cartesModule[2].cartesTypes] += 1;

                for (int i = 0; i < 3; i++)
                {
                    XML_PlaytestAnalyse.NombreDeFoisQuuneCarteEstUtiliseePremierCombat[mm.cartesModule[i].cartesTypes] += 1;
                }

                XML_PlaytestAnalyse.NombreDeFoisSalleUtilisePremierCombat[SalleQuiEffectueAction] += 1;
            }
            else
            {
                XML_PlaytestAnalyse.NombreDeFoisCarteUtiliseeSurUnModuleUnDeuxiemeCombat[mm.cartesModule[0].cartesTypes] += 1;
                XML_PlaytestAnalyse.NombreDeFoisCarteUtiliseeSurUnModuleDeuxDeuxiemeCombat[mm.cartesModule[1].cartesTypes] += 1;
                XML_PlaytestAnalyse.NombreDeFoisCarteUtiliseeSurUnModuleTroisDeuxiemeCombat[mm.cartesModule[2].cartesTypes] += 1;

                for (int i = 0; i < 3; i++)
                {
                    XML_PlaytestAnalyse.NombreDeFoisQuuneCarteEstUtiliseePremierCombat[mm.cartesModule[i].cartesTypes] += 1;
                }

                XML_PlaytestAnalyse.NombreDeFoisSalleUtiliseDeuxiemeCombat[SalleQuiEffectueAction] += 1;
            }
        }

        float cooldown = 0;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Attack
        if (wantedName == "Tourelle BK-1") // ATK
        {
            if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
            {
                if (!XML_PlaytestAnalyse.firstFight)
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat1[0] += 1;
                }
                else
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat2[0] += 1;
                }
            }

            float damage = 35;
            cooldown = 11;
            if (superEffect)
            {
                if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
                {
                    if (!XML_PlaytestAnalyse.firstFight)
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat1 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat1[0] += 1;
                    }
                    else
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat2 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat2[0] += 1;
                    }
                }

                if (mm.cartesModule[2].cartesTypes == 0)
                {
                    damage = OverdriveEfficacity(mm.cartesModule[2].rarity, damage);
                }
                else if (mm.cartesModule[2].cartesTypes == 1)
                {
                    OverdriveTempo(mm.cartesModule[2].rarity, mm.MySalleNumber);
                }
                else if (mm.cartesModule[2].cartesTypes == 2)
                {
                    OverdriveRepetition();
                }
            }
            string effet = "Tire une rafale";
            if (ennemiManager.ennemiRooms[wantedRoom].reflect)
            {
                salleManager.DamageSurSalle(mm.MySalleNumber, 35);
            }

            damage *= ennemiManager.ennemiRooms[wantedRoom].projectileReduction;
            damage *= ennemiManager.ennemiRooms[wantedRoom].cannalisationReduction;

            DealDamage(wantedRoom, damage);
            ennemiManager.animators[wantedRoom].SetTrigger("hit");
            print(effet + " " + damage);
        }
        else if (wantedName == "Tourelle BK-2") // ATK
        {
            if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
            {
                if (!XML_PlaytestAnalyse.firstFight)
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat1[1] += 1;
                }
                else
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat2[1] += 1;
                }
            }

            float damage = 27;
            cooldown = 14;
            if (superEffect)
            {
                if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
                {
                    if (!XML_PlaytestAnalyse.firstFight)
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat1 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat1[1] += 1;
                    }
                    else
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat2 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat2[1] += 1;
                    }
                }

                if (mm.cartesModule[2].cartesTypes == 0)
                {
                    damage = OverdriveEfficacity(mm.cartesModule[2].rarity, damage);
                }
                else if (mm.cartesModule[2].cartesTypes == 1)
                {
                    OverdriveTempo(mm.cartesModule[2].rarity, mm.MySalleNumber);
                }
                else if (mm.cartesModule[2].cartesTypes == 2)
                {
                    OverdriveRepetition();
                }
            }

            string effet = "Tire deux rafales";
            if (ennemiManager.ennemiRooms[wantedRoom].reflect)
            {
                salleManager.DamageSurSalle(mm.MySalleNumber, damage * 2);
            }

            damage *= ennemiManager.ennemiRooms[wantedRoom].projectileReduction;
            damage *= ennemiManager.ennemiRooms[wantedRoom].cannalisationReduction;
            DealDamage(wantedRoom, damage);
            DealDamage(wantedRoom, damage);

            ennemiManager.animators[wantedRoom].SetTrigger("hit");
            print(effet + " " + damage);
        }
        else if (wantedName == "CanonIEM") // ATK
        {
            if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
            {
                if (!XML_PlaytestAnalyse.firstFight)
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat1[4] += 1;
                }
                else
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat2[4] += 1;
                }
            }

            float debuffDuration = 18;
            if (superEffect)
            {
                if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
                {
                    if (!XML_PlaytestAnalyse.firstFight)
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat1 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat1[2] += 1;
                    }
                    else
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat2 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat2[2] += 1;
                    }
                }

                if (mm.cartesModule[2].cartesTypes == 0)
                {
                    debuffDuration = OverdriveEfficacity(mm.cartesModule[2].rarity, debuffDuration);
                }
                else if (mm.cartesModule[2].cartesTypes == 1)
                {
                    OverdriveTempo(mm.cartesModule[2].rarity, mm.MySalleNumber);
                }
                else if (mm.cartesModule[2].cartesTypes == 2)
                {
                    OverdriveRepetition();
                }
            }

            if (ennemiManager.ennemiRooms[wantedRoom].reflect)
            {
                string[] a = new string[] { "DURATION" };
                salleManager.AddEffets(4, "Tempo", a, mm.MySalleNumber, -0.5f);
            }
            if (ennemiManager.actionPrevues.Exists(item => item.origine == wantedRoom))
            {
                int test = ennemiManager.actionPrevues.Find(item => item.origine == wantedRoom).origine;
                ennemiManager.actionPrevues[test].timer += debuffDuration;
                ennemiManager.ennemiRooms[wantedRoom].timer += debuffDuration;
                ennemiManager.animators[wantedRoom].SetTrigger("hit");
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
            if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
            {
                if (!XML_PlaytestAnalyse.firstFight)
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat1[5] += 1;
                }
                else
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat2[5] += 1;
                }
            }

            float buffDuration = 10;
            if (superEffect)
            {
                if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
                {
                    if (!XML_PlaytestAnalyse.firstFight)
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat1 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat1[3] += 1;
                    }
                    else
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat2 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat2[3] += 1;
                    }
                }


                if (mm.cartesModule[2].cartesTypes == 0)
                {
                    buffDuration = OverdriveEfficacity(mm.cartesModule[2].rarity, buffDuration);
                }
                else if (mm.cartesModule[2].cartesTypes == 1)
                {
                    OverdriveTempo(mm.cartesModule[2].rarity, mm.MySalleNumber);
                }
                else if (mm.cartesModule[2].cartesTypes == 2)
                {
                    OverdriveRepetition();
                }
            }

            int x = 0;
            for (int i = 0; i < salleManager.allSalles.Count; i++)
            {
                if (salleManager.allSalles[i].canBeTarget)
                    x++;
            }
            if (x > 1)
            {
                salleManager.allSalles[wantedRoom].canBeTarget = false;
            }

            string[] a = new string[] { "DURATION", "ELECTRONIC" };
            salleManager.AddEffets(buffDuration, "Brouilleur", a, wantedRoom, 0);
            cooldown = 22;
            string effet = "Empêche le ciblage ennemi";
            print(effet);
        }
        else if (wantedName == "Turbine") // DEF
        {
            if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
            {
                if (!XML_PlaytestAnalyse.firstFight)
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat1[2] += 1;
                }
                else
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat2[2] += 1;
                }
            }

            float buffDuration = 6;
            if (superEffect)
            {
                if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
                {
                    if (!XML_PlaytestAnalyse.firstFight)
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat1 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat1[4] += 1;
                    }
                    else
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat2 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat2[4] += 1;
                    }
                }

                if (mm.cartesModule[2].cartesTypes == 0)
                {
                    buffDuration = OverdriveEfficacity(mm.cartesModule[2].rarity, buffDuration);
                }
                else if (mm.cartesModule[2].cartesTypes == 1)
                {
                    OverdriveTempo(mm.cartesModule[2].rarity, mm.MySalleNumber);
                }
                else if (mm.cartesModule[2].cartesTypes == 2)
                {
                    OverdriveRepetition();
                }
            }

            ModuleManager moduleManager = salleManager.allSalles[wantedRoom].MyGo.GetComponent<ModuleManager>();
            if (moduleManager.cartesModule.Count > 1)
            {
                moduleManager.cartesModule[1].durability += 1;
            }
            string[] a = new string[] { "DURATION" };
            salleManager.AddEffets(buffDuration, "Tempo", a, wantedRoom, 0.3f);
            cooldown = 11;
            string effet = "Evacue la pression des moteurs et accélère le temps de recharge";
            print(effet);
        }
        else if (wantedName == "hgOS") // DEF
        {
            if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
            {
                if (!XML_PlaytestAnalyse.firstFight)
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat1[3] += 1;
                }
                else
                {
                    XML_PlaytestAnalyse.CompteurUtilisationEquipementCombat2[3] += 1;
                }
            }

            float amountTime = 8;
            if (superEffect)
            {
                if (!XML_PlaytestAnalyse.CompteUneSeuleAction)
                {
                    if (!XML_PlaytestAnalyse.firstFight)
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat1 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat1[5] += 1;
                    }
                    else
                    {
                        XML_PlaytestAnalyse.NumberOfOverdriveCombat2 += 1;
                        XML_PlaytestAnalyse.NumberOverdriveSetupsCombat2[5] += 1;
                    }
                }

                if (mm.cartesModule[2].cartesTypes == 0)
                {
                    amountTime = OverdriveEfficacity(mm.cartesModule[2].rarity, amountTime);
                }
                else if (mm.cartesModule[2].cartesTypes == 1)
                {
                    OverdriveTempo(mm.cartesModule[2].rarity, mm.MySalleNumber);
                }
                else if (mm.cartesModule[2].cartesTypes == 2)
                {
                    OverdriveRepetition();
                }
            }

            // check si wanted room a des effets electronic
            if (salleManager.allEffets.Exists(item => item.salle == wantedRoom && item.tags.Contains("ELECTRONIC")))
            {
                var temp = salleManager.allEffets.FindAll(item => item.salle == wantedRoom && item.tags.Contains("ELECTRONIC"));
                for (int i = 0; i < temp.Count; i++)
                {
                    temp[i].duration -= amountTime;
                }
            }
            cooldown = 24;
            string effet = "Programme qui purge des virus";
            print(effet);
        }
        salleManager.EnterCooldown(SalleQuiEffectueAction, cooldown);

        if (firstActionTimer == 0 || firstActionTimer <= secondActionTimer)
        {
            firstActionTimer = Time.time;
        }
        else if (secondActionTimer == 0 || secondActionTimer <= firstActionTimer)
        {
            secondActionTimer = Time.time;
        }

        if (firstActionTimer != 0 && secondActionTimer != 0)
        {
            float timerToGive = Mathf.Abs(secondActionTimer - firstActionTimer);
            XML_PlaytestAnalyse.TimeBetweenTwoOperations.Add(timerToGive);
        }

        XML_PlaytestAnalyse.CompteUneSeuleAction = true;
    }

    public void DealDamage(int wantedRoom, float damage)
    {
        if (ennemiManager.badGuy != null)
        {
            ennemiManager.PerdrePvGlobal(damage);
            ennemiManager.PerdrePvLocal(wantedRoom, damage);
            ennemiManager.CheckPdv();
        }
    }

    float OverdriveEfficacity(int x, float value)
    {
        value *= (1 + 0.3f * x);
        return value;
    }

    void OverdriveTempo(int x, int index)
    {
        string[] a = new string[] { "DURATION" };
        salleManager.AddEffets(6, "Tempo", a, index, x * 0.3f);
    }

    void OverdriveRepetition()
    {

    }
}
