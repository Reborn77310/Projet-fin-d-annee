using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupsAttaque
{
    public TextMesh MyText;
    public string MyName;
    public GameObject MyObject;
    
    public SetupsAttaque(string wantedName,GameObject wantedObject)
    {
        MyName = wantedName;
        MyObject = wantedObject;
        
        MyText = MyObject.transform.GetChild(1).GetComponent<TextMesh>();
        MyText.text = MyName;
        MyText.fontSize = 2;
    }
}

public class SetupsDefense
{
    public TextMesh MyText;
    public string MyName;
    public GameObject MyObject;
    
    public SetupsDefense(string wantedName,GameObject wantedObject)
    {
        MyName = wantedName;
        MyObject = wantedObject;
        
        MyText = MyObject.transform.GetChild(1).GetComponent<TextMesh>();
        MyText.text = MyName;
        MyText.fontSize = 2;
    }
}

public class SetupsAlteration
{
    public TextMesh MyText;
    public string MyName;
    public GameObject MyObject;
    
    public SetupsAlteration(string wantedName,GameObject wantedObject)
    {
        MyName = wantedName;
        MyObject = wantedObject;
        
        MyText = MyObject.transform.GetChild(1).GetComponent<TextMesh>();
        MyText.text = MyName;
        MyText.fontSize = 2;
    }
}

public class SetupManager : MonoBehaviour
{
    public string[] AttackNames = new string[5];
    public int wantedAttack = 0;
    public string[] DefenseNames = new string[4];
    public int wantedDefense = 0;
    public string[] AlterationsNames = new string[6];
    public int wantedAlteration = 0;
    
    public SetupsAttaque MySetupAttack;
    public SetupsDefense MySetupDefense;
    public SetupsAlteration MySetupAlteration;
    
    private TextMesh myText;
    
    void Awake()
    {
        MySetupAttack = new SetupsAttaque(AttackNames[wantedAttack],transform.GetChild(0).gameObject);
        MySetupDefense = new SetupsDefense(DefenseNames[wantedDefense],transform.GetChild(1).gameObject);
        MySetupAlteration = new SetupsAlteration(AlterationsNames[wantedAlteration],transform.GetChild(2).gameObject);
    }

    void Update()
    {
        
    }
}
