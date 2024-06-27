using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneHealth : EnemyHealth
{

    protected override IEnumerator DeathCoroutine(string nick)
    {
        isAlive = false;
        GetComponent<EnemyAI>().enabled = false;
        
        GameManager.master.AddScore(nick, score);
        view.RPC("DeathRPC", RpcTarget.All);
        yield return new WaitForSeconds(10);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    protected override void DeathRPC()
    {
        view.enabled = false;
        GetComponent<PhotonTransformView>().enabled = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.velocity = Random.insideUnitSphere * 5;
        StartCoroutine(DisableCoroutine());
        GetComponent<AudioSource>().pitch = Random.Range(0.6f, 1.6f);
        GetComponent<AudioSource>().PlayOneShot(deathSFX, deathVolume);
    }

    IEnumerator DisableCoroutine()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().isKinematic = true;
    }

}
