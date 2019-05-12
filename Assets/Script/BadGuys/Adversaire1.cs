using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Adversaire1 : MonoBehaviour
{
    public Sprite[] normales;
    public Sprite[] highlighted;
    public RectTransform[] ui;
    public float[] pvSalles;
    public float pvTotaux;
    public TextMeshProUGUI[] textPV;
    public Image[] symbolesSalles;
    public int[] formuleTotale;
    public int[] nbSymbolesParSalle;
    EnnemiManager ennemiManager;

    private void Awake()
    {
        ennemiManager = GameObject.Find("GameMaster").GetComponent<EnnemiManager>();
        for (int i = 0; i < ui.Length; i++)
        {
            ennemiManager.RecupInfosADV(ui[i], normales[i], highlighted[i], pvSalles[i], textPV[i]);
        }
        ennemiManager.pvTotaux = pvTotaux;
        ennemiManager.pvTotauxMax = pvTotaux;
        ennemiManager.badGuy = gameObject;
        ennemiManager.RecupFormule(nbSymbolesParSalle, formuleTotale, symbolesSalles);

    }




    // Fonctions pour ses moves -> Comment on va faire pour choisir parmis les fonctions ?

    // Prefab de l'adversaire contient les éléments d'ui et ce script
    // Lorsque le prefab est instancier, ce script passe à ennemi manager la valeur de ses variables
}
