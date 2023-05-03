using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MenuLogic : MonoBehaviourPunCallbacks
{
    [SerializeField] Button createButton;
    [SerializeField] Button joinRandomButton;
    [SerializeField] TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        createButton.interactable = false;
        joinRandomButton.interactable = false;

        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        createButton.onClick.AddListener(CreateRoom);
        joinRandomButton.onClick.AddListener(JoinRandomRoom);
    }

    public override void OnConnectedToMaster()
    {
        createButton.interactable = true;
        joinRandomButton.interactable = true;
        print("Connect To Master Server");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Internet Troubles. Please restart the game");
        print(cause);
    }

    string GetName()
    {
        string name = inputField.text;
        if (string.IsNullOrWhiteSpace(name))
        {
            name = "Player " + Random.Range(0, 9999);
        }
        return name;
    }

    void CreateRoom()
    {
        PhotonNetwork.NickName = GetName();
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        options.IsOpen = true;

        PhotonNetwork.CreateRoom(null, options);
    }

    void JoinRandomRoom()
    {
        PhotonNetwork.NickName = GetName();
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        print(PhotonNetwork.NickName + " joined the room");
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("Error on Join: " + returnCode);
        print(message);
    }




}
