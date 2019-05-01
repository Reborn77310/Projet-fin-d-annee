using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn
{
    public GameObject go;
    public int id;
    public int type;

    public Spawn(GameObject _go, int _id, int _type)
    {
        go = _go;
        id = _id;
        type = _type;
    }
}

public class InstantiatePictos : MonoBehaviour
{

    public int ChanceOfSpawnBleu = 33;
    public int ChanceOfSpawnOrange = 66;
    public int ChanceOfSpawnRouge = 100;

    public List<Spawn> allSpawn = new List<Spawn>();
    public GameObject prefab;
    public GameObject canvas;
    public Transform ParentOfSpawn;
    public int MinGenRandInclusiv = 2;
    public int MaxGenRandExclusiv = 5;

    public int TimeRandMinInclusiv = 10;
    public int TimeRandMaxExclusiv = 25;

    void Start()
    {
        StartCoroutine("Generate");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GeneratePictos();
        }
    }

    IEnumerator Generate()
    {
        float randomTimer = Random.Range(TimeRandMinInclusiv, TimeRandMaxExclusiv);
        yield return new WaitForSeconds(randomTimer);
        GeneratePictos();
        StartCoroutine("Generate");
        yield break;
    }

    public void GeneratePictos()
    {
        int number = Random.Range(MinGenRandInclusiv, MaxGenRandExclusiv);
        number += allSpawn.Count;
        for (int i = allSpawn.Count; i < number; i++)
        {
            GameObject a = Instantiate(prefab, Vector3.zero, Quaternion.identity, ParentOfSpawn);
            PictoManager pm = a.GetComponent<PictoManager>();
            a.GetComponent<RectTransform>().anchoredPosition = new Vector3(Random.Range(250, 900), Random.Range(495, 800), 0);

            int b = Random.Range(1, 101);
            int wantedPrefab = 0;
            if (b >= 1 && b < ChanceOfSpawnBleu)
            {
                wantedPrefab = 0;
            }
            else if (b >= ChanceOfSpawnBleu && b < ChanceOfSpawnOrange)
            {
                wantedPrefab = 1;
            }
            else if (b >= ChanceOfSpawnOrange && b < ChanceOfSpawnRouge)
            {
                wantedPrefab = 2;
            }

            a.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Cartes/Picto/Picto" + wantedPrefab);
            Spawn s = new Spawn(a, i, wantedPrefab);
            pm.id = i;
            pm.type = wantedPrefab;
            allSpawn.Add(s);
        }
    }

    public void DeletePicto(int idToDelete)
    {
        for (int i = idToDelete + 1; i < allSpawn.Count; i++)
        {
            allSpawn[i].go.GetComponent<PictoManager>().id--;
        }
        Destroy(allSpawn[idToDelete].go);
        allSpawn.RemoveAt(idToDelete);
    }
}
