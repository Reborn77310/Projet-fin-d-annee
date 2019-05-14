using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class parent : MonoBehaviour
{
    RectTransform[] zones;
    public Canvas myCanvas;

    void Awake()
    {
        myCanvas = GameObject.Find("GameMaster").GetComponent<GameMaster>().myCanvas;
        zones = GetComponentsInChildren<RectTransform>();
    }

    public bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }

    public int[] CheckOverlap(RectTransform[] rt)
    {
        int[] toReturn = new int[4];
        for (int i = 0; i < rt.Length; i++)
        {
            toReturn[i] = -1;
            for (int h = 0; h < zones.Length; h++)
            {
                if (rectOverlaps(rt[i], zones[h]))
                {
                    toReturn[i] = i;
                    h = zones.Length;
                }
            }
        }
        return toReturn;
    }


    public void RotationZones(Transform t)
    {
        // The mighty trick : Technique très ancienne V2
        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].SetParent(gameObject.transform, true);
        }
        transform.rotation = t.rotation;
        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].SetParent(myCanvas.transform, true);
        }
    }
}
