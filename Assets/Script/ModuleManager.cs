using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 

// public class Modules
// {
    
//     public enum Type
//     {
//         Gauche,Milieu,Droite
//     }

//     public int CarteIndex;
//     public GameObject MyObject;
//     public Type MyType;
//     public Modules(GameObject mO,Type _moduleTypes)
//     {
//         MyObject = mO;
//         MyType = _moduleTypes;
//         //CarteIndex = _CarteIndex;
//     }

    
// }

public class ModuleManager : MonoBehaviour
{
    public GameObject[] MyModules = new GameObject[3];
    public List<CartesManager.Cartes> cartesModule = new List<CartesManager.Cartes>();
    public TextMesh MyCompteur;
    public int MyCompteurInt = 2;
    public int MySalleNumber;
    public int rotationCompteur = 400000;
    
    void Awake()
    {
        MyModules[0] = transform.GetChild(0).gameObject;
        MyModules[1] = transform.GetChild(1).gameObject;
        MyModules[2] = transform.GetChild(2).gameObject;

        MyCompteur.text = MyCompteurInt.ToString();
        
    }
}
