using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class WaitPanel : MonoBehaviour
{
    [SerializeField] TMP_Text infoText;
    [SerializeField] Button startGameButton;
    [SerializeField] Button exitButton;

    // Start is called before the first frame update
    void Start()
    {

        if(PhotonNetwork.IsMasterClient == false) startGameButton.gameObject.SetActive(false);
    }

    public void DisplayInfo(string info)
    {
        infoText.text = info;
    }




    
}
