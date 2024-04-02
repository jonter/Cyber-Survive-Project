using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class BossRocketSkill : MonoBehaviour
{
    PhotonView view;
    [SerializeField] ParticleSystem aimVFX;
    [SerializeField] GameObject rocket;
    [SerializeField] ParticleSystem burstVFX;

    [SerializeField] float timeBeforeBoom = 2;

    [SerializeField] float burstRadius = 4;
    [SerializeField] float burstDamage = 50;

    

    // Start is called before the first frame update
    IEnumerator Start()
    {
        view = GetComponent<PhotonView>(); 
        aimVFX.Play();
        rocket.transform.DOMoveY(2, timeBeforeBoom).SetEase(Ease.Linear);
        yield return new WaitForSeconds(timeBeforeBoom);
        aimVFX.Stop();
        burstVFX.transform.parent = null;
        burstVFX.Play();
        rocket.SetActive(false);
        if (PhotonNetwork.IsMasterClient == false) yield break;
        DamageAll();
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, burstRadius);
    }

    void DamageAll()
    {
        PlayerHealth[] players = FindObjectsOfType<PlayerHealth>();
        for (int i = 0; i < players.Length; i++)
        {
            float distance = Vector3.Distance(transform.position,
                players[i].transform.position);
            if (distance <= burstRadius)
            {
                PhotonView pView = players[i].GetComponent<PhotonView>();
                pView.RPC("GetDamage", pView.Owner, burstDamage);
            }
        }
    }

   
}
