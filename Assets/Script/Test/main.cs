using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class main : MonoBehaviour
{

    void Start()
    {
        Generate2();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Draw(3);
        }
    }
    public GameObject prefab;
    public int prefabWantedNumber;
    public List<GameObject> liste = new List<GameObject>();
    public Canvas canvas;

    public void Generate2()
    {
        float startingXpos = ((prefabWantedNumber - 1) / 2) * -70;
        startingXpos -= 419;
        for (int i = 0; i < prefabWantedNumber; i++)
        {
            liste.Add(Instantiate(prefab, Vector3.zero, Quaternion.identity, canvas.transform));

            float gap = (-((prefabWantedNumber - 1.0f) / 2.0f) + i);
            float angleToRotate = CalculGap(Mathf.Abs(gap));

            float y = angleToRotate * -4;
            y -= 480;
            liste[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(startingXpos + i * 70, y, 0);

            if (gap > 0)
            {
                angleToRotate = -angleToRotate;
            }
            liste[i].transform.Rotate(new Vector3(0, 0, angleToRotate));
        }
    }

    float CalculGap(float max)
    {
        float toReturn = max;
        for (int i = 0; i < max; i++)
        {
            toReturn += i;
        }
        return toReturn;
    }

    public void Draw(float cardsToDraw)
    {
        for (int i = 0; i < cardsToDraw; i++)
        {
            liste.Add(Instantiate(prefab, Vector3.zero, Quaternion.identity, canvas.transform));
        }
        SortCards();
    }

    public void SortCards()
    {
        float startingXpos = ((liste.Count - 1) / 2) * -70;
        startingXpos -= 419;
        for (int i = 0; i < liste.Count; i++)
        {
            float gap = (-((liste.Count - 1.0f) / 2.0f) + i);
            float angleToRotate = CalculGap(Mathf.Abs(gap));

            float y = angleToRotate * -4;
            y -= 480;
            liste[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(startingXpos + i * 70, y, 0);

            if (gap > 0)
            {
                angleToRotate = -angleToRotate;
            }
            liste[i].transform.rotation = Quaternion.Euler(0,0,0);
            liste[i].transform.Rotate(new Vector3(0, 0, angleToRotate));
        }
    }

}
