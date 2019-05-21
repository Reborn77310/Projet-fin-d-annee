using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleLog : MonoBehaviour
{
    // "\n"
        // <color=#BC1910> normale
        // <color=#7841BB> intégrité
        // <color=#E6742E> attaque
        // <color=#4061BA> defense
        // <color=#6289F3> buff
        // <color=#B97D31> debuff
        // </color>


    public static TextMeshProUGUI textInfosAction;
    public int id;

    private void Awake() {
        textInfosAction = GameObject.Find("Infos text").GetComponent<TextMeshProUGUI>();
    }

    public static void ChangeText(string a)
    {
        textInfosAction.text = a;
    }
}
