using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    float maxHp = 100;
    float hp;

    [SerializeField] Slider healthBar;
    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        healthBar.value = hp / maxHp;
        view = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void GetDamage(float damage)
    {
        hp -= damage;
        view.RPC("DisplayHealth", RpcTarget.All, hp);

        if(hp <= 0.001f)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    void DisplayHealth(float newHP)
    {
        healthBar.value =  newHP / maxHp;
    }



   
}
