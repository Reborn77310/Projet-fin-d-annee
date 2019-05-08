using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modules
{
    public enum Type
    {
        Gauche,Milieu,Droite
    }

    public int CarteType;
    public GameObject MyObject;
    public Type MyType;
    public Modules(GameObject mO,Type _moduleTypes, int _CarteType)
    {
        MyObject = mO;
        MyType = _moduleTypes;
        CarteType = _CarteType;
    }

    public int rotationCompteur = 400000;
}

public class ModuleManager : MonoBehaviour
{
    public Modules[] MyModules = new Modules[3];
    public TextMesh MyCompteur;
    public int MyCompteurInt = 2;
    
    void Awake()
    {
        MyModules[0] = new Modules(transform.GetChild(0).gameObject,Modules.Type.Gauche,-1);
        MyModules[1] = new Modules(transform.GetChild(1).gameObject,Modules.Type.Milieu,-1);
        MyModules[2] = new Modules(transform.GetChild(2).gameObject,Modules.Type.Droite,-1);

        MyCompteur.text = MyCompteurInt.ToString();
    }
}
