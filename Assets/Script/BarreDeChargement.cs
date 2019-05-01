using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarreDeChargement : MonoBehaviour
{

    public float maxTimer;
    float timer;
    public Image fill;
    public bool isLoading = false;
    public int cartesJouees = 0;
    public Text textNBcartesJouees;
	public Text textScoreDuJoueur;
    public List<int> salleIdJouees = new List<int>();
    public int carteType = -1;
	public int score;
	private InstantiatePictos IP;
	
    private void Awake()
    {
	    IP = GetComponent<InstantiatePictos>();
    }

    private void Update()
    {
        if (isLoading)
        {
            OnLoading();
        }
    }

    public void OnLoading()
    {
        timer += Time.deltaTime;
        fill.fillAmount = timer / maxTimer;
        if (timer >= maxTimer)
        {
			score += ScoreCalculation(cartesJouees);
			textScoreDuJoueur.text = "Score de " + PlayerPrefs.GetString("PlayerName") + ": " + score.ToString();
            fill.fillAmount = 0;
            isLoading = false;
            cartesJouees = 0;
            textNBcartesJouees.text = cartesJouees.ToString();
	        
	        for (int i = 0; i < IP.allSpawn.Count; i++)
	        {
		        if (IP.allSpawn[i].type == carteType)
		        {
			        IP.allSpawn[i].go.GetComponent<PictoManager>().ReactualiseList();
		        }
	        }
	        
            carteType = -1;
            for (int i = 0; i < salleIdJouees.Count; i++)
            {
                //cartesManager.allSalles[salleIdJouees[i]].salleScript.cardHasBeenPlayedHere = false;
            }
            salleIdJouees.Clear();
        }
    }

    public void Reset(int salleID)
    {
        timer = 0;
        fill.fillAmount = 0;
        isLoading = true;
        cartesJouees++;
        textNBcartesJouees.text = cartesJouees.ToString();
        salleIdJouees.Add(salleID);

	    for (int i = 0; i < IP.allSpawn.Count; i++)
	    {
		    if (IP.allSpawn[i].type == carteType && IP.allSpawn[i].go.GetComponent<RectTransform>().transform.position.y <= 1080)
		    {
			    //IP.allSpawn[i].go.GetComponent<PictoManager>().CheckCombo(cartesManager.allSalles[salleID].salleGO.transform.GetChild(3).name);
		    }
	    }
	    
       // cartesManager.allSalles[salleID].salleScript.mr.material.color = Color.white;
        /*if (cartesManager.allSalles[salleID].salleScript.mr.gameObject.transform.childCount > 0)
        {
            cartesManager.allSalles[salleID].salleScript.mr.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color
            = Color.white;
        }*/
		
        if (cartesJouees == 1)
        {
            if (carteType == 0)
            {
                fill.color = Color.blue;
            }
            else if (carteType == 1)
            {
                fill.color = Color.yellow;
            }
            else
            {
                fill.color = Color.red;
            }
        }
    }

	public int ScoreCalculation(int nbCartes)
	{
		int toReturn = 0;
		if (nbCartes == 1)
		{
			toReturn = 1;
		}
		else if (nbCartes == 2)
		{
			toReturn = 3;
		}
		else if (nbCartes == 3)
		{
			toReturn = 8;
		}
		else if (nbCartes == 4)
		{
			toReturn = 30;
		}
		else if (nbCartes == 5)
		{
			toReturn = 100;
		}
		return toReturn;
	}
}
