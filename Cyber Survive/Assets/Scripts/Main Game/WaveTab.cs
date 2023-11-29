using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveTab : MonoBehaviour
{
    
    public void SetText(string str)
    {
        GetComponentInChildren<TMP_Text>().text = str;
    }

}
