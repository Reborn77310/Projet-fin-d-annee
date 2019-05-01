using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PictoManager : MonoBehaviour
{
	public int id;
	public int type;
	public List<string> Combos = new List<string>();
	private Text myText;
	InstantiatePictos IP;
	RectTransform limit;
    public float Speed;
    bool stopped = false;

	public int Compteur = 0;
	public List<string> WantedCombo = new List<string>();
	private List<string> actualList = new List<string>();
    void Start()
    {
	    Combos.Add("Exca");
	    Combos.Add("Nav");
	    Combos.Add("Rad");
	    Combos.Add("Tech");
	    Combos.Add("Def");
	    myText = transform.GetChild(0).GetComponent<Text>();
	    int random = Random.Range(2, 5);
	    for (int i = 0; i < random; i++)
	    {
		    int rand = Random.Range(0,5-i);
		    WantedCombo.Add(Combos[rand]);
		    Combos.RemoveAt(rand);
	    }

	    if (random == 2)
	    {
		    myText.text = WantedCombo[0] + "/" + WantedCombo[1];
	    }
	    else if (random == 3)
	    {
		    myText.text = WantedCombo[0] + "/" + WantedCombo[1] + "/" + WantedCombo[2];
	    }
	    else if (random == 4)
	    {
		    myText.text = WantedCombo[0] + "/" + WantedCombo[1] + "/" + WantedCombo[2] + "/" + WantedCombo[3];
	    }
		IP = GameObject.Find("GameMaster").GetComponent<InstantiatePictos>();
		limit = GameObject.Find("Limit").GetComponent<RectTransform>();
	    ReactualiseList();
    }

    void Update()
    {
        if (!stopped)
        {
            transform.Translate(-transform.up * Time.deltaTime * Speed);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            stopped = !stopped;
        }

		if(GetComponent<RectTransform>().transform.position.y <= limit.transform.position.y)
		{
			IP.DeletePicto(id);
		}
    }

	public void ValidatePicto()
	{
		IP.DeletePicto(id);
	}


	public void ReactualiseList()
	{
		actualList.Clear();
		myText.color = Color.white;
		for (int i = 0; i < WantedCombo.Count; i++)
		{
			actualList.Add(WantedCombo[i]);
		}
		ActualiseText();
	}
	
	public void CheckCombo(string salleName)
	{
		for (int a = 0; a < actualList.Count; a++)
		{
			if (actualList[a] == salleName)
			{
				actualList.RemoveAt(a);
				myText.color = Color.green;
			}
		}
		ActualiseText();
	}

	public void ActualiseText()
	{
		if (actualList.Count == 0)
		{
			ScoreToGive();
			ValidatePicto();
		}
		else if (actualList.Count == 1)
		{
			myText.text = actualList[0];
		}
		else if (actualList.Count == 2)
		{
			myText.text = actualList[0] + "/" + actualList[1];
		}
		else if (actualList.Count == 3)
		{
			myText.text = WantedCombo[0] + "/" + WantedCombo[1] + "/" + WantedCombo[2];
		}
		else if (actualList.Count == 4)
		{
			myText.text = WantedCombo[0] + "/" + WantedCombo[1] + "/" + WantedCombo[2] + "/" + WantedCombo[3];
		}
	}

	void ScoreToGive()
	{
		GameObject.Find("GameMaster").GetComponent<BarreDeChargement>().score += WantedCombo.Count * 50;
	}
}
