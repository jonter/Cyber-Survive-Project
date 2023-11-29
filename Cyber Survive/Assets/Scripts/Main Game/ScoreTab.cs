using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTab : MonoBehaviour
{
    public void SetText(string str)
    {
        TMP_Text mytext = GetComponentInChildren<TMP_Text>();
        mytext.text = str;
    }

    
}
