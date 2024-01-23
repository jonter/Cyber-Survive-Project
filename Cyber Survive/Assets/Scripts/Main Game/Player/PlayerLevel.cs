using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;
    PhotonView view;
    int level;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine == false) return;
        level = PlayerPrefs.GetInt("level");
        view.RPC("DisplayLevel", RpcTarget.All, level);
        
    }

    [PunRPC]
    void DisplayLevel(int level)
    {
        levelText.text = $"[level {level + 1}]";
    }

    
}
