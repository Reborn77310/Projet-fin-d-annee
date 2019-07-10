using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuleManager : MonoBehaviour
{
    public GameObject[] MyModules;
    public List<CartesManager.Cartes> cartesModule = new List<CartesManager.Cartes>();
    public int MyCompteurInt = 2;
    public int MySalleNumber;
    public int rotationCompteur = 400000;
    public Sprite[] defaultSprite;
    public Image[] slotImage;

    private void Awake() {
        defaultSprite = new Sprite[3];
        slotImage = new Image[3];
        for (int i = 0; i < 3; i++)
        {
            defaultSprite[i] = MyModules[i].GetComponent<Image>().sprite;
            slotImage[i] = MyModules[i].GetComponent<Image>();
        }
    }

}
