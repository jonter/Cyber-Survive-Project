using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCharacter : MonoBehaviour
{
    [SerializeField] Button upgradesButton;

    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text charNameText;

    int price = 99999;

    [Tooltip("ѕод каким ID сохран€етс€ уровень прокачки персонажа")]
    [SerializeField] string charID = "trooper";

    [SerializeField] string charName = "Trooper";
    private void OnEnable()
    {
        upgradesButton.onClick.AddListener(Upgrade);
        DisplayLevelAndPrice();
        charNameText.text = $"[{charName}]";
    }

    private void OnDisable()
    {
        upgradesButton.onClick.RemoveListener(Upgrade);
    }

    void DisplayLevelAndPrice()
    {
        int level = PlayerPrefs.GetInt(charID);
        level++;
        levelText.text = "Level " + level;
        price = level * 100;
        priceText.text = $"Upgrade ({price})";
    }

    void Upgrade()
    {
        MenuCreditManager creditManager = FindObjectOfType<MenuCreditManager>();
        bool isSuccess = creditManager.SpendMoney(price);

        if (isSuccess == false) return;

        int level = PlayerPrefs.GetInt(charID);
        PlayerPrefs.SetInt(charID, level + 1);
        DisplayLevelAndPrice();
    }


}
