using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuCreditManager : MonoBehaviour
{
    TMP_Text creditText;

    // Start is called before the first frame update
    void Start()
    {
        int credits = PlayerPrefs.GetInt("credits");
        creditText = GetComponentInChildren<TMP_Text>(); 
        creditText.text = "" + credits;
    }

    public bool SpendMoney(int price)
    {
        int credits = PlayerPrefs.GetInt("credits");
        if (credits < price) return false;

        PlayerPrefs.SetInt("credits", credits - price);
        creditText.text = "" + (credits-price);
        return true;
    }

    
}
