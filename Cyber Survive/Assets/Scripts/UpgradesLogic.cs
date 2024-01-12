using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesLogic : MonoBehaviour
{
    [SerializeField] Button openButton;
    [SerializeField] Button closeButton;
    [SerializeField] Button upgradesButton;

    [SerializeField] GameObject upgradesPanel;

    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text priceText;

    int price = 99999;

    // Start is called before the first frame update
    void Start()
    {
        upgradesPanel.SetActive(false);
        openButton.onClick.AddListener(ShowUpgrades);
        closeButton.onClick.AddListener(HideUpgrades);
        upgradesButton.onClick.AddListener(Upgrade);
    }

    void ShowUpgrades()
    {
        upgradesPanel.SetActive(true);
        DisplayLevelAndPrice();
    }

    void DisplayLevelAndPrice()
    {
        int level = PlayerPrefs.GetInt("level");
        level++;
        levelText.text = "Level "+ level;
        price = level * 100;
        priceText.text = $"Upgrade ({price})";
    }

    void HideUpgrades()
    {
        upgradesPanel.SetActive(false);
    }

    void Upgrade()
    {
        MenuCreditManager creditManager = FindObjectOfType<MenuCreditManager>();
        bool isSuccess = creditManager.SpendMoney(price);

        if (isSuccess == false) return;

        int level = PlayerPrefs.GetInt("level");
        PlayerPrefs.SetInt("level", level + 1);
        DisplayLevelAndPrice();
    }

}
