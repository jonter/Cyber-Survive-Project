using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossAI : EnemyAI
{
    float timeBeforeAbility = 0;
    BoxCollider damageTrigger;
    float damage = 40;
    ParticleSystem fireVFX;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }


    protected override void Start()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        base.Start();
        GetComponentInChildren<BossDamageTrigger>().damage = damage;
        damageTrigger = GetComponentInChildren<BossDamageTrigger>()
            .GetComponent<BoxCollider>();

        damageTrigger.enabled = false;
        fireVFX = damageTrigger.GetComponent<ParticleSystem>();
    }


    protected override void Update()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        if (target == null) return;
        if (isAction == true) return;

        timeBeforeAbility += Time.deltaTime;
        agent.SetDestination(target.position);
        float distance = Vector3.Distance(transform.position, target.position);

        if(timeBeforeAbility >= 15)
        {
            // сделать вызов способности  (топнуть)
            timeBeforeAbility = 0;
        }
        else if (distance <= attackDistance)
        {
            StartCoroutine(AttackCoroutine());
        }
        HandleMoveAnim();
    }

    void HandleMoveAnim()
    {
        if(agent.desiredVelocity.magnitude <= 0.2f)
            anim.SetBool("shoot", true);
        else
            anim.SetBool("shoot", false);
    }

    IEnumerator AttackCoroutine()
    {
        agent.SetDestination(transform.position);
        isAction = true;
        timeBeforeAbility += 2;
        Vector3 startRot = new Vector3(0, -45, 0);
        transform.DORotate(startRot, 0.3f, RotateMode.WorldAxisAdd);
        yield return null;
        anim.SetBool("shoot", true);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(ShootCoroutine());
        yield return new WaitForSeconds(2f);
        isAction = false;
    }

    IEnumerator ShootCoroutine()
    {
        fireVFX.Play();
        yield return new WaitForSeconds(0.1f);
        damageTrigger.enabled = true;
        Vector3 rot = new Vector3(0, 90, 0);
        transform.DORotate(rot, 1.5f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(1.5f);
        fireVFX.Stop();
        damageTrigger.enabled = false;
        rot = new Vector3(0, -45, 0);
        transform.DORotate(rot, 0.3f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.InOutSine);
    }

}
