using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharManager : MonoBehaviour
{
    [SerializeField] Image classImage;
    [SerializeField] Button leftButton, rightButton;

    [SerializeField] GameObject[] classPrefabs;
    [SerializeField] Sprite[] classIcons;

    int index = 0;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (classPrefabs.Length != classIcons.Length) 
            Debug.LogError("Incorrect Icons and Classes");

        gameManager = GetComponentInParent<GameManager>();
        gameManager.playerPrefab = classPrefabs[index];
        classImage.sprite = classIcons[index];

        leftButton.onClick.AddListener(DecreaseIndex);
        rightButton.onClick.AddListener(IncreaseIndex);
    }

    void IncreaseIndex()
    {
        index++;
        if (index >= classIcons.Length) index = 0;
        gameManager.playerPrefab = classPrefabs[index];
        classImage.sprite = classIcons[index];
    }

    void DecreaseIndex()
    {
        index--;
        if (index < 0) index = classIcons.Length - 1;
        gameManager.playerPrefab = classPrefabs[index];
        classImage.sprite = classIcons[index];
    }

}
