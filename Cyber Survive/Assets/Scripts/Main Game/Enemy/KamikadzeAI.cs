using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class KamikadzeAI : EnemyAI
{
    protected override void Update()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        if (target == null) return;
        if (isAction == true) return;

        agent.SetDestination(target.position);
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance < attackDistance)
        {
            isAction = false;
            agent.SetDestination(transform.position);
            StartCoroutine(GetComponent<KamikadzeHealth>().MakeBoomCoroutine());
        }

    }
}
