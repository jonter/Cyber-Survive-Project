using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hp = 20;
    float maxHP;
    PhotonView view;

    Animator anim;
    bool isAlive = true;

    [SerializeField] Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHP = hp;
        healthBar.value = hp / maxHP;
        view = GetComponent<PhotonView>();
        anim = GetComponentInChildren<Animator>();
    }

    [PunRPC]
    void GetDamage(float damage)
    {
        if (isAlive == false) return;
        hp -= damage;
        view.RPC("UpdateEnemyHealthBar", RpcTarget.All, (hp/maxHP));

        if(hp <= 0.0001f)
        {
            StartCoroutine(DeathCoroutine());
        }
    }

    [PunRPC]
    void UpdateEnemyHealthBar(float percentage)
    {
        healthBar.value = percentage;
        if (percentage <= 0)
            Destroy(healthBar.gameObject);
    }

    IEnumerator DeathCoroutine()
    {
        isAlive = false;
        int rand = Random.Range(0, 2);
        if (rand == 0) anim.SetTrigger("death1");
        else anim.SetTrigger("death2");
        GetComponent<EnemyAI>().enabled = false;
        view.RPC("DisableColliders", RpcTarget.All);
        yield return new WaitForSeconds(10);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void DisableColliders()
    {
        GetComponent<Collider>().enabled = false;
    }
    
}
