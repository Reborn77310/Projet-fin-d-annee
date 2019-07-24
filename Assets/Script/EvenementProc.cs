using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenementProc : MonoBehaviour
{
   public GameObject NeigeTemporaire;
   public GameObject TransmissionCadre;
   public GameObject perso1;
   public GameObject perso2;

   private bool firstFight = false;
   //Différents types d'événements possibles : Combat, Dialogue
  
   public bool ActuallySeeking = false;
   private float randomTime = 0;

   public void FindNewTimer(float maxTimer)
   {
      randomTime = Random.Range(5, maxTimer);
      ActuallySeeking = true;
   }

   private void Update()
   {
      if (ActuallySeeking && CartesManager.PhaseLente)
      {
         if (randomTime > 0)
         {
            randomTime -= Time.deltaTime;
         }
         else
         {
            randomTime = 0;
            ActuallySeeking = false;
            if (!firstFight)
            {
               CombatEncounter();
               firstFight = true;
            }
            else
            {
               BossFight();
            }
         }
      }
   }

   void CombatEncounter()
   {
      var emission = NeigeTemporaire.GetComponent<ParticleSystem>().emission;
      emission.enabled = false;

      for (int i = 0; i < 4; i++)
      {
         GameObject.Find("PattesController").GetComponent<PlayWithDécalage>().AllLegs[i].speed = 0;
      }
      TransmissionCadre.SetActive(true);
      perso1.GetComponent<MovieTexturePersoUn>().Activevideo();
      Camera.main.GetComponent<MusicSound>().StopPhaseLente();
   }

   void BossFight()
   {
      PlayWithDécalage.CanMove = false;
      TransmissionCadre.SetActive(true);
      TransmissionCadre.transform.GetChild(2).GetComponent<Dialogue>().isActive = true;
      Camera.main.GetComponent<MusicSound>().StopPhaseLente();
      perso2.GetComponent<MovieTexturePersoDeux>().Activevideo();
   }
}
