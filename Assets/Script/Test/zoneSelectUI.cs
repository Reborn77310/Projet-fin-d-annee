using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class zoneSelectUI : MonoBehaviour
{
    // FAIRE BOUGER LES ZONES A PARTIR D'UN PARENT = PAS DETECTION OVERLAP
    // FAIRE ROTATE LES ZONES A PARTIR D'UN PARENT = PAS DETECTION OVERLAP
    // CONCLUSION : PREFAB CONTENANT LES ZONES (4 ROTATION)
    // MOLETTE = CHANGEMENT PREFAB (BOUGER DUN INDEX)
    // APRES SPAWN CHECK OVERLAP
    // AUTRE SOLUTION : NUMEROTER LES CASES, LES SALLES ONT DES ID CORESPONDANT A LEUR CASES, IDEM POUR LES ZONES ET ON COMPARE

    public RectTransform[] uiRect1; //Zones ciblage RectTransform
    public GameObject Salles; //parent des salles
    public RectTransform[] uiRect2; //Salles RectTransform

    private void Start()
    {
        uiRect1 = GetComponentsInChildren<RectTransform>();
        uiRect2 = Salles.GetComponentsInChildren<RectTransform>();
    }

    void Update()
    {
        MoveUI();
        CheckOverlapRoom();
    }

    public void MoveUI()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            uiRect1[0].position += new Vector3(0, 100, 0);
            CheckOverlapRoom();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            uiRect1[0].position += new Vector3(0, -100, 0);
            CheckOverlapRoom();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            uiRect1[0].position += new Vector3(-100, 0, 0);
            CheckOverlapRoom();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            uiRect1[1].position += new Vector3(100, 0, 0);
            CheckOverlapRoom();
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            transform.Rotate(0,0,90);
            CheckOverlapRoom();
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            transform.Rotate(0,0,-90);
            CheckOverlapRoom();
        }
    }

    public bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }

    public void CheckOverlapRoom()
    {
        // for (int i = 0; i < uiRect2.Length; i++)
        // {
        //     uiRect2[i].gameObject.GetComponent<Image>().color = Color.white;
        //     for (int h = 1; h < uiRect1.Length; h++)
        //     {
        //         if(rectOverlaps(uiRect2[i], uiRect1[h]))
        //         {
        //             Debug.Log(uiRect1[h].name + " " +uiRect2[i].name);
        //             uiRect2[i].gameObject.GetComponent<Image>().color = Color.red;
        //             h = uiRect1.Length;
        //         }
        //     }
        // }

        if (rectOverlaps(uiRect2[0], uiRect1[1]))
        {
            print("a");
        }



    }

    public void ChangeColor(bool overlap, RectTransform rt)
    {
        if (overlap)
        {
            rt.gameObject.GetComponent<Image>().color = Color.red;

        }
        else
        {
            rt.gameObject.GetComponent<Image>().color = Color.white;
        }
    }
}
