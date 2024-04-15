using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class MenuLogic : MonoBehaviourPunCallbacks
{
    [SerializeField] Button openCreatePanelButton;
    [SerializeField] Button openJoinPanelButton;
    [SerializeField] TMP_InputField inputField;

    [SerializeField] GameObject createRoomPanel;
    [SerializeField] GameObject joinRoomPanel;

    [SerializeField] Button closeCreatePanelButton;
    [SerializeField] Button closeJoinPanelButton;

    [Header("Настройки присоединения к комнате")]
    [SerializeField] GameObject joinButtonPrefab;
    [SerializeField] GameObject content;
    [SerializeField] GameObject warningText;

    List<JoinRoomButton> roomButtons;

    [SerializeField] ErrorPanel errorPanel;

    // Start is called before the first frame update
    void Start()
    {
        roomButtons = new List<JoinRoomButton>(); 
        openCreatePanelButton.interactable = false;
        openJoinPanelButton.interactable = false;

        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        openCreatePanelButton.onClick.AddListener(OpenCreatePanel);
        closeCreatePanelButton.onClick.AddListener(CloseCreatePanel);
        openJoinPanelButton.onClick.AddListener(OpenJoinPanel);
        closeJoinPanelButton.onClick.AddListener(CloseJoinPanel);
    }

    void OpenCreatePanel() 
    {
        PhotonNetwork.NickName = GetName();
        createRoomPanel.SetActive(true);
    }
    void OpenJoinPanel() 
    {
        PhotonNetwork.NickName = GetName();
        joinRoomPanel.SetActive(true);
    }

    void CloseCreatePanel() { createRoomPanel.SetActive(false); }
    void CloseJoinPanel() { joinRoomPanel.SetActive(false); }

    public override void OnConnectedToMaster()
    {
        
        print("Connect To Master Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Connect to Lobby. Now we can create/join Rooms");
        openCreatePanelButton.interactable = true;
        openJoinPanelButton.interactable = true;
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            JoinRoomButton updatedButton = null;
            foreach(JoinRoomButton button in roomButtons)
            {
                if (button.roomInfo.Name == info.Name) 
                    updatedButton = button;
            }
            if(updatedButton == null) 
            {
                GameObject clone = Instantiate(joinButtonPrefab, content.transform);
                JoinRoomButton btn = clone.GetComponent<JoinRoomButton>();
                btn.SetupButton(info);
                roomButtons.Add(btn);
            }
            else if (info.RemovedFromList == true)
            {
                Destroy(updatedButton.gameObject);
                roomButtons.Remove(updatedButton);
            }
            else
            {
                updatedButton.SetupButton(info);
            }
        }

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Internet Troubles. Please restart the game");
        errorPanel.gameObject.SetActive(true);
        errorPanel.SetupPanel(cause.ToString());
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
