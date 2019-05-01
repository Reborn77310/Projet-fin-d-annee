using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPref : MonoBehaviour
{
    public Button[] Niveaux;
    public InputField MyInputField;
    public Text MyText;
    BarreDeChargement Br;

    void Start()
    {
        Br = GameObject.Find("GameMaster").GetComponent<BarreDeChargement>();
        SetNewPlayer();
        Br.textScoreDuJoueur.text = "Score de " + PlayerPrefs.GetString("PlayerName") + ": " + Br.score;
    }

    public void SaveName()
    {
        string playerName = MyInputField.text;
        if (MyInputField.text != "" && MyText.text == "New Player")
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            MyText.text = PlayerPrefs.GetString("PlayerName");
            Br.textScoreDuJoueur.text = "Score de " + PlayerPrefs.GetString("PlayerName") + ": " + Br.score;
        }
        if (PlayerPrefs.HasKey("PlayerName") && MyText.text != "New Player")
        {
            PlayerPrefs.SetInt(PlayerPrefs.GetString("PlayerName") + "_Score", Br.score);
        }
    }

    public void SetNewPlayer()
    {
        Br.score = 0;

        PlayerPrefs.SetString("PlayerName", "New Player");
        MyText.text = "New Player";
        Br.textScoreDuJoueur.text = "Score de " + PlayerPrefs.GetString("PlayerName") + ": " + Br.score;
    }

    public void LoadScore()
    {
        string previous = PlayerPrefs.GetString("PlayerName");
        PlayerPrefs.SetString("PlayerName", MyInputField.text);
        if (PlayerPrefs.GetInt(PlayerPrefs.GetString("PlayerName") + "_Score") != 0)
        {
            Br.score = PlayerPrefs.GetInt(PlayerPrefs.GetString("PlayerName") + "_Score");
            Br.textScoreDuJoueur.text = "Score de " + PlayerPrefs.GetString("PlayerName") + ": " + Br.score;
            MyText.text = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            PlayerPrefs.SetString("PlayerName", previous);
        }
    }

    public void CleanInputField()
    {
        StartCoroutine("Clean");
    }

    IEnumerator Clean()
    {
        yield return new WaitForSeconds(0.2f);
        MyInputField.text = "";
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        SetNewPlayer();
    }
}
