using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hp = 20;
    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    [PunRPC]
    void GetDamage(float damage)
    {
        hp -= damage;

        if(hp <= 0.0001f)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    
}
