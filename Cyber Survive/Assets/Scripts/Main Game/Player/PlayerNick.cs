using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNick : MonoBehaviour
{
    [SerializeField] PhotonView playerView;
    // Start is called before the first frame update
    void Start()
    {
        TMP_Text myText = GetComponent<TMP_Text>();
        myText.text = playerView.Owner.NickName;
        if(playerView.IsMine == false)
        {
            myText.color = new Color(0.24f, 0.91f, 0.71f, 1);
        }
    }

    
}
