using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : EnemyAI
{

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletSpeed = 15;
    [SerializeField] float bulletDamage = 15;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }


    protected override void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
        if (PhotonNetwork.IsMasterClient == false) return;
        StartCoroutine(ChoosePlayerToChase());
    }

    protected override void Update()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        if (target == null) return;
        if (isAction == true) return;

        agent.SetDestination(target.position);
        if (CanShoot() == false) return;

        StartCoroutine(MakeShoot());

    }

    IEnumerator MakeShoot()
    {
        agent.SetDestination(transform.position);
        isAction = true;
        StartCoroutine(RotateToPlayer());
        yield return new WaitForSeconds(0.5f);
        Quaternion rot = Quaternion.LookRotation(transform.forward);
        GameObject clone = PhotonNetwork.Instantiate(bulletPrefab.name,
            transform.position, rot);
        Vector3 velocity = transform.forward * bulletSpeed;
        clone.GetComponent<DroneBullet>().Launch(bulletDamage, velocity);
        yield return new WaitForSeconds(0.5f);
        isAction = false;
    }


    bool CanShoot()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackDistance) return false;
        Vector3 origin1 = transform.position +transform.right * 0.3f;
        Vector3 origin2 = transform.position -transform.right * 0.3f;

        Vector3 dir = target.position - transform.position;
        LayerMask env = LayerMask.GetMask("Env");
        bool isHit1 = Physics.Raycast(origin1, dir, distance, env);
        bool isHit2 = Physics.Raycast(origin2, dir, distance, env);

        if(isHit1 == true ||  isHit2 == true) return false;

        return true;
    }


}
