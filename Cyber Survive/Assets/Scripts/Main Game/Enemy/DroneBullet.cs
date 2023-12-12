using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : Bullet
{

    protected override void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        
        if (other.GetComponent<PlayerHealth>())
        {
            PhotonView playerView = other.GetComponent<PhotonView>();
            playerView.RPC("GetDamage", playerView.Owner, damage);
        }
        PhotonNetwork.Destroy(gameObject);
    }

}
