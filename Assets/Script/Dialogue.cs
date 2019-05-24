using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    TextMeshProUGUI dialogue;
    string[] _textCharacter;
    public bool isActive = false;
    public float timeInSeconds;
    float timer;
    int characterCount;

    private void Awake()
    {
        dialogue = GetComponent<TextMeshProUGUI>();
        _textCharacter = new string[dialogue.text.Length];
        for (int i = 0; i < dialogue.text.Length; i++)
        {
            _textCharacter[i] = dialogue.text.Substring(i, 1);
        }
        dialogue.text = "";
        characterCount = 0;
        timer = 0;
    }

    private void Update()
    {
        if (isActive)
        {
            if (characterCount < _textCharacter.Length)
            {
                timer += Time.deltaTime;
                if (timer >= timeInSeconds)
                {
                    dialogue.text += _textCharacter[characterCount];
                    characterCount++;
                    timer = 0;
                }
            }
            if (characterCount == _textCharacter.Length)
            {
                StartCoroutine("attend", 1.5f);
                characterCount++;
            }
        }
        else
        {
            dialogue.text = "";
        }
    }

    IEnumerator attend(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (transform.childCount > 0)
        {
            transform.GetChild(0).GetComponent<Dialogue>().isActive = true;
        }
        else
        {
            // Appeler la fonction que tu veux, dialogue fini
        }
        isActive = false;
    }
}
