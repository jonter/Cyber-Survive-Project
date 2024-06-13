using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;


[SelectionBase]
public class Grenade : MonoBehaviour
{
    float damage = 30;
    [SerializeField] float burstRadius = 2;
    PhotonView view;

    [SerializeField] GameObject burstVFX;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, burstRadius);
    }

    public void Launch(Vector3 dest, float damage, float flyTime)
    {
        view = GetComponent<PhotonView>();
        this.damage = damage;
        transform.DOMoveX(dest.x, flyTime).SetEase(Ease.Linear);
        transform.DOMoveZ(dest.z, flyTime).SetEase(Ease.Linear);
        transform.DOMoveY(dest.y, flyTime).SetEase(Ease.InBack);// могут тут быть проблемы
        Vector3 rot = new Vector3(40, 0, 0);
        transform.DORotate(rot, flyTime, RotateMode.LocalAxisAdd).SetEase(Ease.InBack);
        StartCoroutine(BurstWithDelay(flyTime));
    }

    IEnumerator BurstWithDelay(float t)
    {
        yield return new WaitForSeconds(t);
        MakeBoom();
    }

    private void OnDisable()
    {
        GameObject cloneVFX = Instantiate(burstVFX, transform.position, Quaternion.identity);
        cloneVFX.GetComponent<ParticleSystem>().Play();
        

        transform.DOKill();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (view == null) return;
        if(view.IsMine == false) return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy)
        {
            PhotonView enView = enemy.GetComponent<PhotonView>();
            enView.RPC("GetDamage", RpcTarget.MasterClient, damage, view.Owner.NickName);
        }

        MakeBoom();
    }


    void MakeBoom()
    {
        Collider[] objs = Physics.OverlapSphere(transform.position, burstRadius);
        foreach (Collider obj in objs)
        {
            EnemyHealth enemy = obj.GetComponent<EnemyHealth>();
            if(enemy)
            {
                PhotonView enView = enemy.GetComponent<PhotonView>();
                enView.RPC("GetDamage", RpcTarget.MasterClient, damage, view.Owner.NickName);
            }
            PlayerHealth plr = obj.GetComponent<PlayerHealth>();
            if (plr)
            {
                PhotonView pView = plr.GetComponent<PhotonView>();
                if(pView.Owner.NickName == view.Owner.NickName)
                {
                    plr.GetDamage(damage/2);
                }
            }
        }

        PhotonNetwork.Destroy(gameObject);
    }



}
