using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MakePrepotGreatAgain : MonoBehaviour
{
    public Vector3 PrepotRoom1 = Vector3.zero;
    public Vector3 PrepotRoom2 = Vector3.zero;
    public Vector3 PrepotRoom3 = Vector3.zero;
    public Vector3 PrepotRoom4 = Vector3.zero;
    private CartesManager cm;
    private EnnemiManager em;
    public GameObject mob;
    public GameObject boss;
    
    public GameObject MobPrefab;
    public GameObject BossPrefab;
    
    void Start()
    {
        em = GameObject.Find("GameMaster").GetComponent<EnnemiManager>();
        cm = GameObject.Find("GameMaster").GetComponent<CartesManager>();
        
    }

    void LaunchPrepot()
    {
        cm.Prepot(Mathf.RoundToInt(PrepotRoom1.x),Mathf.RoundToInt(PrepotRoom1.y),Mathf.RoundToInt(PrepotRoom1.z));
        cm.Prepot(Mathf.RoundToInt(PrepotRoom2.x),Mathf.RoundToInt(PrepotRoom2.y),Mathf.RoundToInt(PrepotRoom2.z));
        cm.Prepot(Mathf.RoundToInt(PrepotRoom3.x),Mathf.RoundToInt(PrepotRoom3.y),Mathf.RoundToInt(PrepotRoom3.z));
        cm.Prepot(Mathf.RoundToInt(PrepotRoom4.x),Mathf.RoundToInt(PrepotRoom4.y),Mathf.RoundToInt(PrepotRoom4.z));
        cm.DrawCards();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Camera.main.GetComponent<MusicSound>().StopCombat();
            CartesManager.PhaseLente = true;
            GameMaster.SceneTest = false;
            Application.LoadLevel(SceneManager.GetActiveScene().name);
        }
    }
    
    public void StartPlayerChoice()
    {
        mob.SetActive(true);
        boss.SetActive(true);
    }

    public void ClickedOnChoice(GameObject wantedName)
    {
        string name = wantedName.name;
        if (name == "Mob")
        {
            em.prefabADV = MobPrefab;
        }
        else
        {
            em.prefabADV = BossPrefab;
        }
        StartCombat();
        mob.SetActive(false);
        boss.SetActive(false);
    }
    void StartCombat()
    {
        var gameMaster = GameObject.Find("GameMaster");
        
        Camera.main.GetComponent<MusicSound>().LancerMusiqueCombat();
        gameMaster.GetComponent<EnnemiManager>().SpawnAdversaire();

        Camera.main.GetComponent<Animator>().SetTrigger("GoDown");
        int count = gameMaster.GetComponent<Equipement>().allEquipements.Count;
        for (int i = 0; i < count; i++)
        {
            gameMaster.GetComponent<Equipement>().allEquipements[i].go.GetComponent<Animator>().SetTrigger("Sortirlestourelles");
        }
        LaunchPrepot();
    }
}
