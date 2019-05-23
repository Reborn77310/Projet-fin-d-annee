using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoutonsScript : MonoBehaviour
{
    public GameObject[] Buttons = new GameObject[4];
    public GameObject[] Buttons2 = new GameObject[4];

    public void AfficherLesTextes(Image go)
    {

        for (int i = 0; i < 4; i++)
        {
            if (Buttons[i].gameObject != null)
            {
                if (go.gameObject == Buttons[i])
                {
                    if (Buttons[i].transform.childCount > 0)
                    {
                        if (Buttons[i].transform.GetChild(0).gameObject.activeInHierarchy)
                        {
                            Buttons[i].transform.GetChild(0).gameObject.SetActive(false);
                        }
                        else
                        {
                            Buttons[i].transform.GetChild(0).gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    if (Buttons[i].transform.childCount > 0)
                    {
                        Buttons[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public void AfficherLesTextes2(Image go)
    {

        for (int i = 0; i < 4; i++)
        {
            if (Buttons2[i].gameObject != null)
            {
                if (go.gameObject == Buttons2[i])
                {
                    if (Buttons2[i].transform.childCount > 0)
                    {
                        if (Buttons2[i].transform.GetChild(0).gameObject.activeInHierarchy)
                        {
                            Buttons2[i].transform.GetChild(0).gameObject.SetActive(false);
                        }
                        else
                        {
                            Buttons2[i].transform.GetChild(0).gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    if (Buttons2[i].transform.childCount > 0)
                    {
                        Buttons2[i].transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
