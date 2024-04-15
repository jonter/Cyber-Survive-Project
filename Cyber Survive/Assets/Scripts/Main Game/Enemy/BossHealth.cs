using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BossHealth : EnemyHealth
{

    [PunRPC]
    protected override void DeathRPC()
    {
        isAlive = false;
        anim.SetTrigger("death");
        GetComponent<Collider>().enabled = false;
    }

}
