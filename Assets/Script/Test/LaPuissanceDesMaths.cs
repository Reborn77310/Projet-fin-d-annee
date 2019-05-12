using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class LaPuissanceDesMaths : MonoBehaviour
{
    public struct cb
    {
        public int[] nbGrille;
        public Image image;
    }


    public GameObject salles;
    public List<cb> mesCarreBlancs = new List<cb>();
    int[] Rota0 = new int[] { 2, 6, 7, 8 };
    int[] Rota1 = new int[] { 12, 16, 17, 22 };
    int[] Rota2 = new int[] { 18, 19, 20, 24 };
    int[] Rota3 = new int[] { 4, 9, 14, 15 };


    public int actuelleRotation = 0;

    public void Initialisation()
    {
        cb a = new cb();
        a.nbGrille = new int[] { 6, 7 };
        a.image = salles.transform.GetChild(0).GetComponent<Image>();
        mesCarreBlancs.Add(a);

        cb b = new cb();
        b.nbGrille = new int[] { 5 };
        b.image = salles.transform.GetChild(1).GetComponent<Image>();
        mesCarreBlancs.Add(b);

        cb c = new cb();
        c.nbGrille = new int[] { 18, 23 };
        c.image = salles.transform.GetChild(2).GetComponent<Image>();
        mesCarreBlancs.Add(c);


    }

    void Start()
    {
        Initialisation();
    }

    void Update()
    {
        Rotationne();
    }

    void AplyRotationne()
    {
        if (actuelleRotation == 0)
        {
            GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (actuelleRotation == 1)
        {
            GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (actuelleRotation == 2)
        {
            GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (actuelleRotation == 3)
        {
            GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90);
        }
        CheckCollisionSalles();
    }

    void Rotationne()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (actuelleRotation > 0)
            {
                actuelleRotation -= 1;
            }
            else
            {
                actuelleRotation = 3;
            }
            AplyRotationne();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (actuelleRotation < 3)
            {
                actuelleRotation += 1;
            }
            else
            {
                actuelleRotation = 0;
            }
            AplyRotationne();
        }
    }

    void CheckCollisionSalles()
    {
        for (int i = 0; i < mesCarreBlancs.Count; i++)
        {
            mesCarreBlancs[i].image.color = Color.white;
            for (int h = 0; h < mesCarreBlancs[i].nbGrille.Length; h++)
            {
                if (actuelleRotation == 0)
                {
                    if (Rota0.Contains(mesCarreBlancs[i].nbGrille[h]))
                    {
                        mesCarreBlancs[i].image.color = Color.red;
                        h = mesCarreBlancs[i].nbGrille.Length;
                    }
                }
                else if (actuelleRotation == 1)
                {
                    if (Rota1.Contains(mesCarreBlancs[i].nbGrille[h]))
                    {
                        mesCarreBlancs[i].image.color = Color.red;
                        h = mesCarreBlancs[i].nbGrille.Length;
                    }
                }
                else if (actuelleRotation == 2)
                {
                    if (Rota2.Contains(mesCarreBlancs[i].nbGrille[h]))
                    {
                        mesCarreBlancs[i].image.color = Color.red;
                        h = mesCarreBlancs[i].nbGrille.Length;
                    }
                }
                else if (actuelleRotation == 3)
                {
                    if (Rota3.Contains(mesCarreBlancs[i].nbGrille[h]))
                    {
                        mesCarreBlancs[i].image.color = Color.red;
                        h = mesCarreBlancs[i].nbGrille.Length;
                    }
                }
            }
        }
    }
}
