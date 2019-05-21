﻿using System.Collections;
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


    public TextMeshProUGUI textInfosActionADV;
    public TextMeshProUGUI textInfosActionNEST;

    public void ChangeTextInfosADV(string a)
    {
        textInfosActionADV.text = a;
    }
    public void ChangeTextInfosNEST(string a)
    {
        textInfosActionNEST.text = a;
    }
}