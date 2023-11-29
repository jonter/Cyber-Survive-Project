using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ExitGameLogic : MonoBehaviour
{
    [SerializeField] GameObject exitPanel;

    [SerializeField] Button exitButton;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    // Start is called before the first frame update
    void Start()
    {
        exitPanel.SetActive(false);

        exitButton.onClick.AddListener(OnExitButton);
        yesButton.onClick.AddListener(OnYesButton);
        noButton.onClick.AddListener(OnNoButton);
    }

    void OnExitButton()
    {
        exitPanel.SetActive(true);
    }

    void OnYesButton()
    {
        PhotonNetwork.LeaveRoom();
    }

    void OnNoButton()
    {
        exitPanel.SetActive(false);
    }


    
}
