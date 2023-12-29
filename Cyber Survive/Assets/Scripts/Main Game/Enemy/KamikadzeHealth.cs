using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KamikadzeHealth : EnemyHealth
{
    [SerializeField] float burstRadius = 3;
    [SerializeField] float burstDamage = 50;
    [SerializeField] GameObject burstPrefab;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, burstRadius);
    }

    protected override IEnumerator DeathCoroutine(string nick)
    {
        isAlive = false;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Vector3 velocity = agent.desiredVelocity;
        gameObject.AddComponent<Rigidbody>().velocity = velocity / 2;
        GetComponent<EnemyAI>().enabled = false;
        //agent.enabled = false;
        GameManager.master.AddScore(nick, score);
        
        yield return StartCoroutine(MakeBoomCoroutine());
    }

    public IEnumerator MakeBoomCoroutine()
    {
        view.RPC("PlayEffect", RpcTarget.All);
        yield return new WaitForSeconds(0.7f);
        PhotonNetwork.Instantiate(burstPrefab.name, transform.position, 
            Quaternion.identity);
        PlayerHealth[] players = FindObjectsOfType<PlayerHealth>();
        for (int i = 0; i < players.Length; i++)
        {
            float distance = Vector3.Distance(transform.position,
                players[i].transform.position);
            if(distance <= burstRadius)
            {
                PhotonView pView = players[i].GetComponent<PhotonView>();
                pView.RPC("GetDamage", pView.Owner, burstDamage);
            }
        }
        PhotonNetwork.Destroy(gameObject);
    }


    [PunRPC]
    void PlayEffect()
    {
        GetComponentInChildren<ParticleSystem>().Play();
    }




}
