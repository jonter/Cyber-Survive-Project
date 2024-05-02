using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HostLeftPanel : MonoBehaviour
{
    [SerializeField] TMP_Text creditsText;

    public void SetCreditText(int credits)
    {
        creditsText.text = $"Вы заработали {credits} кредитов за сессию";
    }    
    
}
