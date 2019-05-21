using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Voydroc : MonoBehaviour
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
    public int[] nbActionsParSalle;
    public int[] actions;
    EnnemiManager ennemiManager;
    public Sprite[] formule;

    private void Awake()
    {
        ennemiManager = GameObject.Find("GameMaster").GetComponent<EnnemiManager>();
        for (int i = 0; i < ui.Length; i++)
        {
            ennemiManager.RecupInfosADV(ui[i], normales[i], highlighted[i], pvSalles[i], textPV[i]);
        }
        ennemiManager.pvTotaux = pvTotaux;
        ennemiManager.pvTotauxMax = pvTotaux;
        ennemiManager.pvTotauxText.gameObject.SetActive(true);
        ennemiManager.badGuy = gameObject;
        ennemiManager.RecupFormule(nbSymbolesParSalle, formuleTotale, symbolesSalles);
        ennemiManager.RecupActions(nbActionsParSalle, actions);
        ennemiManager.ApplyFormule(formule);
    }
}
