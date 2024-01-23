using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateRoomLogic : MonoBehaviour
{
    [SerializeField] TMP_InputField inputRoomName;
    [SerializeField] TMP_Text playerCountText;

    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] Button createButton;

    int playerCount = 4;

    // Start is called before the first frame update
    void Start()
    {
        playerCountText.text = "" + playerCount;
        leftButton.onClick.AddListener(DecreasePlayerCount);
        rightButton.onClick.AddListener(IncreasePlayerCount); 
        createButton.onClick.AddListener(CreateRoom);
    }

    void DecreasePlayerCount()
    {
        playerCount--;
        if (playerCount < 2) playerCount = 2;
        playerCountText.text = "" + playerCount;
    }

    void IncreasePlayerCount()
    {
        playerCount++;
        if (playerCount > 6) playerCount = 6;
        playerCountText.text = "" + playerCount;
    }

    void CreateRoom()
    {
        createButton.interactable = false;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (byte)playerCount;
        string name = inputRoomName.text;
        if (string.IsNullOrWhiteSpace(name)) name = $"Room {Random.Range(0, 1000)}";
        options.IsOpen = true;
        options.IsVisible = true;
        PhotonNetwork.CreateRoom(name, options);  
    }



}
