using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    protected PhotonView view;
    protected float damage = 10;
    // Start is called before the first frame update
    void Awake()
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (view == null) return;
        if (view.IsMine == false) return;

        if (other.GetComponent<EnemyHealth>())
        {
            PhotonView enemyView = other.GetComponent<PhotonView>();
            enemyView.RPC("GetDamage", RpcTarget.MasterClient, damage, view.Owner.NickName);
        }

        PhotonNetwork.Destroy(view);
    }

}
