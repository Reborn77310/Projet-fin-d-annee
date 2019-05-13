using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleManager : MonoBehaviour
{
    public GameObject[] MyModules;
    public List<CartesManager.Cartes> cartesModule = new List<CartesManager.Cartes>();
    public TextMesh MyCompteur;
    public int MyCompteurInt = 2;
    public int MySalleNumber;
    public int rotationCompteur = 400000;

}
