using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class parent : MonoBehaviour
{
    RectTransform[] zones;
    public Canvas myCanvas;
    RectTransform[] sallesRT;
    public GameObject salles;
    void Start()
    {
        zones = GetComponentsInChildren<RectTransform>();
        sallesRT = salles.GetComponentsInChildren<RectTransform>();
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Mouse0))
        // {
        //     for (int i = 0; i < zones.Length; i++)
        //     {
        //         zones[i].SetParent(gameObject.transform, true);
        //     }
        //     transform.Rotate(0, 0, 90);
        //     for (int i = 0; i < zones.Length; i++)
        //     {
        //         zones[i].SetParent(myCanvas.transform, true);
        //     }
        //     CheckOverlap();
        // }
    }

    public bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }

    public void CheckOverlap()
    {
        for (int i = 0; i < sallesRT.Length; i++)
        {
            sallesRT[i].gameObject.GetComponent<Image>().color = Color.white;
            for (int h = 0; h < zones.Length; h++)
            {
                if (rectOverlaps(sallesRT[i], zones[h]))
                {
                    sallesRT[i].gameObject.GetComponent<Image>().color = Color.red;
                    h = zones.Length;
                }
            }
        }
    }

    public void RotateHoraire()
    {
        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].SetParent(gameObject.transform, true);
        }
        transform.Rotate(0, 0, 90);
        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].SetParent(myCanvas.transform, true);
        }
        CheckOverlap();
    }

    public void RotateAntihoraire()
    {
        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].SetParent(gameObject.transform, true);
        }
        transform.Rotate(0, 0, 90);
        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].SetParent(myCanvas.transform, true);
        }
        CheckOverlap();
    }
}
