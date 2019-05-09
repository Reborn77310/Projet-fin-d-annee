using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salles
{
    public GameObject MyGo;
    public bool CanPlayHere = true;
    public int pv;

    public Salles(GameObject go)
    {
        MyGo = go;
        pv = 100;
    }
}

public class SalleManager : MonoBehaviour
{

    public List<Salles> allSalles = new List<Salles>();
    public int pvDuVehicule = 300;
    void Start()
    {
        InitializeSalles();
    }

    void InitializeSalles()
    {
        for (int i = 0; i < 4; i++)
        {
            Salles newSalle = new Salles(GameObject.Find("Salle" + i.ToString()));
            allSalles.Add(newSalle);
        }
    }

    public void DamageSurSalle(int salleVisee, int damage)
    {
        allSalles[salleVisee].pv -= damage;
        pvDuVehicule -= damage;
        if (pvDuVehicule <= 0)
        {
            Debug.Log("MISSANDEI ?!?");
            Time.timeScale = 0;
        }
        if (allSalles[salleVisee].pv <= 0)
        {
            allSalles[salleVisee].pv = 0;
            allSalles[salleVisee].CanPlayHere = false;
            ReparationSalle(salleVisee);
        }
    }

    IEnumerator ReparationSalle(int salleVisee)
    {
        yield return new WaitForSeconds(30);
        allSalles[salleVisee].CanPlayHere = true;
        allSalles[salleVisee].pv = 100;
        yield break;
    }
}
