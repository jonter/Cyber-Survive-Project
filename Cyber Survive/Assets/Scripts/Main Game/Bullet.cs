using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    PhotonView view;
    float damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    public void Launch(float d, Vector3 velocity)
    {
        damage = d;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = velocity;
        StartCoroutine(DeleteBulletInTime());
    }

    IEnumerator DeleteBulletInTime()
    {
        yield return new WaitForSeconds(10);
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (view.IsMine == false) return;

        if (other.GetComponent<EnemyHealth>())
        {
            PhotonView enemyView = other.GetComponent<PhotonView>();
            enemyView.RPC("GetDamage", RpcTarget.MasterClient, damage);
        }

        PhotonNetwork.Destroy(view);
    }

}
