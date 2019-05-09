using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnnemiRooms
{
    public GameObject[] myRooms = new GameObject[4];
    public int[] myTypes =  new int[4];
    public int[] myPdv = new int[4];
    public Text[] myText = new Text[4];
    public bool[] isDead = new bool[4];
    public EnnemiRooms()
    {
        
    }
}

public class Spawns
{
    public GameObject go;
    public int id;
    public Image myHP;
    public Text myText;
    public int myPV;
    public EnnemiRooms MyChilds  = new EnnemiRooms();
    
    public Spawns(GameObject _go, int _id)
    {
        go = _go;
        id = _id;
        myHP = go.transform.GetChild(6).GetComponent<Image>();
        myPV = 300;
        myHP.fillAmount = myPV;
        myText = myHP.transform.GetChild(0).GetComponent<Text>();
        myText.text = "Hp : " + myPV;
        
        var childs = go.transform.GetChild(5);
        for (int i = 0; i < 4; i++)
        {
            MyChilds.myRooms[i] = childs.transform.GetChild(i).gameObject;
            int randomRange = Random.Range(0, 3);
            MyChilds.myTypes[i] = randomRange;
            MyChilds.myPdv[i] = 100;
            MyChilds.isDead[i] = false;
            MyChilds.myText[i] = _go.transform.GetChild(i).GetComponent<Text>();
            MyChilds.myRooms[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/Picto" + randomRange);
            MyChilds.myText[i].text = "Salle " + i + " = " + MyChilds.myPdv[i] + " pdv.";
        }
    }
}

public class EnnemiManager : MonoBehaviour
{
    public GameObject ParentSpawner;
    public GameObject PrefabSpawn;
    public static List<Spawns> CurrentSpawns = new List<Spawns>();
    public static EnnemiManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnMob()
    {
        var randomPos = Vector3.zero;
        randomPos.y = Random.Range(-226, 231);
        randomPos.x = Random.Range(330, 853);
        var go = Instantiate(PrefabSpawn, Vector3.zero, Quaternion.identity,ParentSpawner.transform);
        go.GetComponent<RectTransform>().anchoredPosition = randomPos;
        Spawns spawn = new Spawns(go,CurrentSpawns.Count);
        CurrentSpawns.Add(spawn);
    }

    public static void PerdrePvLocal(int i,int wantedRoom,int wantedDamage)
    {
        CurrentSpawns[i].MyChilds.myPdv[wantedRoom] -= wantedDamage;
        CurrentSpawns[i].MyChilds.myText[wantedRoom].text = "Salle " + wantedRoom + " = " + CurrentSpawns[i].MyChilds.myPdv[wantedRoom] + " pdv.";

        if (CurrentSpawns[i].MyChilds.myPdv[wantedRoom] <= 0)
        {
            CurrentSpawns[i].MyChilds.isDead[wantedRoom] = true;
            instance.StartCoroutine(instance.RespawnSalle(i,wantedRoom));
        }
    }
    
    public static void PerdrePvGlobal(int numberOfPv,int wantedSpawn)
    {
        CurrentSpawns[wantedSpawn].myPV -= numberOfPv;
        CurrentSpawns[wantedSpawn].myHP.fillAmount = CurrentSpawns[wantedSpawn].myPV/3;
        CurrentSpawns[wantedSpawn].myText.text = "Hp : " + CurrentSpawns[wantedSpawn].myPV;
    }

    IEnumerator RespawnSalle(int i,int wantedSpawn)
    {
        float timer = 20;
        while (timer > 0)
        {
            if (CurrentSpawns.Count > 0)
            {
                CurrentSpawns[i].MyChilds.myText[wantedSpawn].text = "Temps restant :" + timer;
            }
            
            yield return new WaitForSeconds(1);
            timer -= 1;
        }

        if (CurrentSpawns.Count > 0)
        {
            CurrentSpawns[i].MyChilds.isDead[wantedSpawn] = false;
            CurrentSpawns[i].MyChilds.myPdv[wantedSpawn] = 100;
            CurrentSpawns[i].MyChilds.myText[wantedSpawn].text = "Salle " + wantedSpawn + " = " + CurrentSpawns[i].MyChilds.myPdv[wantedSpawn] + " pdv.";
        }
        yield break;
    }

    public static void CheckPdv()
    {
        int count = CurrentSpawns.Count;
        int compteur = 0;
        for (int i = 0; i < count ; i++)
        {
            if (CurrentSpawns[i - compteur].myPV <= 0)
            {
                Destroy(CurrentSpawns[i - compteur].go);
                CurrentSpawns.RemoveAt(i - compteur);
                compteur += 1;
            }
        }
    }
}
