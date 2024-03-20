using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossDamageTrigger : MonoBehaviour
{
    public float damage = 40;


    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player)
        {
            PhotonView playerView  = player.GetComponent<PhotonView>();
            playerView.RPC("GetDamage", playerView.Owner, damage);
        }
        
    }

}
