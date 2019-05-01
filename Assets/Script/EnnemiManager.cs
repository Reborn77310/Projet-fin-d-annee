using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnnemiRooms
{
    public GameObject[] myRooms = new GameObject[4];
    public int[] myTypes =  new int[4];
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
        myHP = go.transform.GetChild(2).GetComponent<Image>();
        myPV = 100;
        myHP.fillAmount = myPV;
        myText = myHP.transform.GetChild(0).GetComponent<Text>();
        myText.text = "Hp : " + myPV;
        
        var childs = go.transform.GetChild(1);
        for (int i = 0; i < 4; i++)
        {
            MyChilds.myRooms[i] = childs.transform.GetChild(i).gameObject;
            int randomRange = Random.Range(0, 3);
            MyChilds.myTypes[i] = randomRange;
            MyChilds.myRooms[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/Picto" + randomRange);
        }
    }
}

public class EnnemiManager : MonoBehaviour
{
    public GameObject ParentSpawner;
    public GameObject PrefabSpawn;
    public static List<Spawns> CurrentSpawns = new List<Spawns>();
    
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

    public static void PerdrePv(int numberOfPv,int wantedSpawn)
    {
        CurrentSpawns[wantedSpawn].myPV -= numberOfPv;
        CurrentSpawns[wantedSpawn].myHP.fillAmount = CurrentSpawns[wantedSpawn].myPV;
        CurrentSpawns[wantedSpawn].myText.text = "Hp : " + CurrentSpawns[wantedSpawn].myPV;
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
