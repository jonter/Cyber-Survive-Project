using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Photon.Pun;


[SelectionBase]
public class Grenade : MonoBehaviour
{
    float damage = 30;
    [SerializeField] float burstRadius = 2;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, burstRadius);
    }

    public void Launch(Vector3 dest, float damage, float flyTime)
    {
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
        ParticleSystem burstVFX = GetComponentInChildren<ParticleSystem>();
        burstVFX.transform.parent = null;
        burstVFX.Play();
        transform.DOKill();
    }



    void MakeBoom()
    {
        PhotonNetwork.Destroy(gameObject);
    }



}
