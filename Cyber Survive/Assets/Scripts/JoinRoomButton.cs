using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class JoinRoomButton : MonoBehaviour
{
    [SerializeField] TMP_Text roomName;
    [SerializeField] TMP_Text playerCountText;

    public RoomInfo roomInfo;
    public void SetupButton(RoomInfo info)
    {
        roomInfo = info;
        roomName.text = info.Name;
        playerCountText.text = $"{info.PlayerCount}/{info.MaxPlayers}";
        if(info.MaxPlayers == info.PlayerCount) playerCountText.color = Color.red;

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(JoinRoom);
    }

    void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

}
