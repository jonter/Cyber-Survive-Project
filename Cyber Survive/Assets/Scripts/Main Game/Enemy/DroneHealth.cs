using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneHealth : EnemyHealth
{

    protected override IEnumerator DeathCoroutine(string nick)
    {
        isAlive = false;
        
        GetComponent<EnemyAI>().enabled = false;
        GameManager.master.AddScore(nick, score);
        view.RPC("DisableColliders", RpcTarget.All);
        yield return new WaitForSeconds(10);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    protected override void DisableColliders()
    {
        view.enabled = false;
        GetComponent<PhotonTransformView>().enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.velocity = Random.insideUnitSphere * 5;
        StartCoroutine(DisableCoroutine());
    }

    IEnumerator DisableCoroutine()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().isKinematic = true;
    }

}
