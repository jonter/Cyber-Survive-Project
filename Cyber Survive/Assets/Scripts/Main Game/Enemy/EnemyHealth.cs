using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] protected float hp = 20;
    [SerializeField] protected int score = 10;
    protected float maxHP;
    protected PhotonView view;

    Animator anim;
    protected bool isAlive = true;

    [SerializeField] protected Slider healthBar;
    // Start is called before the first frame update
    void Start()
    {
        maxHP = hp;
        healthBar.value = hp / maxHP;
        view = GetComponent<PhotonView>();
        anim = GetComponentInChildren<Animator>();
    }

    public void IncreaseHP(float hpMult)
    {
        hp *= hpMult;
        maxHP = hp;
    }

    [PunRPC]
    protected void GetDamage(float damage, string nick)
    {
        if (isAlive == false) return;
        hp -= damage;
        view.RPC("UpdateEnemyHealthBar", RpcTarget.All, (hp/maxHP));

        if(hp <= 0.0001f)
        {
            StartCoroutine(DeathCoroutine(nick));
        }
    }

    [PunRPC]
    protected void UpdateEnemyHealthBar(float percentage)
    {
        healthBar.value = percentage;
        if (percentage <= 0)
            Destroy(healthBar.gameObject);
    }

    protected virtual IEnumerator DeathCoroutine(string nick)
    {
        isAlive = false;
        int rand = Random.Range(0, 2);
        if (rand == 0) anim.SetTrigger("death1");
        else anim.SetTrigger("death2");
        GetComponent<EnemyAI>().enabled = false;
        GameManager.master.AddScore(nick, score);
        view.RPC("DisableColliders", RpcTarget.All);
        yield return new WaitForSeconds(10);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    protected virtual void DisableColliders()
    {
        GetComponent<Collider>().enabled = false;
    }
    
}
