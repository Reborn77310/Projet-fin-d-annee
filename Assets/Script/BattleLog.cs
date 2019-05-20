using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleLog : MonoBehaviour
{
    public static TextMeshProUGUI textInfosAction;

    private void Awake() {
        textInfosAction = GameObject.Find("Infos text").GetComponent<TextMeshProUGUI>();
    }

    public static void ChangeText(string a)
    {
        textInfosAction.text = a;
    }
}
