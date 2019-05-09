using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Salles
{
    public GameObject MyGo;
    public bool CanPlayHere = true;
    public int pv;

    public bool isDefendu;

    public Salles(GameObject go)
    {
        MyGo = go;
        pv = 100;
        isDefendu = false;
    }
}

public class SalleManager : MonoBehaviour
{

    public List<Salles> allSalles = new List<Salles>();
    public GameObject[] pvSalles;
    public TextMesh pvDuVehiculeText;
    public int pvDuVehicule = 300;
    void Start()
    {
        InitializeSalles();
        pvDuVehiculeText.text = pvDuVehicule.ToString();
    }

    void InitializeSalles()
    {
        for (int i = 0; i < 4; i++)
        {
            Salles newSalle = new Salles(GameObject.Find("Salle" + i.ToString()));
            allSalles.Add(newSalle);
            pvSalles[i].GetComponent<TextMesh>().text = allSalles[i].pv.ToString();
        }
    }

    public void DamageSurSalle(int salleVisee, int damage)
    {
        allSalles[salleVisee].pv -= damage;
        pvDuVehicule -= damage;
        pvDuVehiculeText.text = pvDuVehicule.ToString();
        if (pvDuVehicule <= 0)
        {
            Debug.Log("MISSANDEI ?!?");
            Time.timeScale = 0;
        }
        if (allSalles[salleVisee].pv <= 0)
        {
            allSalles[salleVisee].pv = 0;
            allSalles[salleVisee].CanPlayHere = false;
            pvSalles[salleVisee].GetComponent<TextMesh>().text = allSalles[salleVisee].pv.ToString();
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
