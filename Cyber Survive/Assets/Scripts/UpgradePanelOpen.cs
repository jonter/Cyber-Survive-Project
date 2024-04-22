using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePanelOpen : MonoBehaviour
{
    [SerializeField] Button openButton;
    [SerializeField] Button closeButton;

    [SerializeField] GameObject upgradesPanel;
    [SerializeField] GameObject[] charUpgrades;

    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        upgradesPanel.SetActive(false);
        openButton.onClick.AddListener(ShowUpgrades);
        closeButton.onClick.AddListener(HideUpgrades);
        leftButton.onClick.AddListener(DecreaseIndex);
        rightButton.onClick.AddListener(IncreaseIndex);
    }

    void ShowUpgrades()
    {
        upgradesPanel.SetActive(true);
        charUpgrades[index].SetActive(true);
    }

    void HideUpgrades()
    {
        upgradesPanel.SetActive(false);
    }

    void IncreaseIndex()
    {
        charUpgrades[index].SetActive(false);
        index++;
        if (index >= charUpgrades.Length) index = 0;
        charUpgrades[index].SetActive(true);
    }

    void DecreaseIndex()
    {
        charUpgrades[index].SetActive(false);
        index--;
        if (index < 0) index = charUpgrades.Length - 1;
        charUpgrades[index].SetActive(true);
    }


}
