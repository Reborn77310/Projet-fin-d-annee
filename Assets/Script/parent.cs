using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class parent : MonoBehaviour
{
    RectTransform[] zones;
    public Canvas myCanvas;
    EnnemiManager ennemiManager;

    void Awake()
    {
        myCanvas = GameObject.Find("GameMaster").GetComponent<GameMaster>().myCanvas;
        zones = GetComponentsInChildren<RectTransform>();
        ennemiManager = GameObject.Find("GameMaster").GetComponent<EnnemiManager>();
    }

    public bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        var r = rectTrans1.rect;
        r.center = rectTrans1.TransformPoint(r.center);
        r.size = rectTrans1.TransformVector(r.size);

        var r2 = rectTrans2.rect;
        r2.center = rectTrans2.TransformPoint(r2.center);
        r2.size = rectTrans2.TransformVector(r2.size);

        return r.Overlaps(r2);



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
                    zones[h].GetComponent<Image>().color = Color.red;
                    toReturn[i] = i;
                    print(i + " " + ennemiManager.animators.Length);
                    if (ennemiManager.animators.Length > 0)
                    {
                        ennemiManager.animators[i].SetBool("hightlight", true);
                    }

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
            zones[i].rotation = Quaternion.identity;
            zones[i].localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < zones.Length; i++)
        {
            Destroy(zones[i].gameObject);
        }
    }
}
