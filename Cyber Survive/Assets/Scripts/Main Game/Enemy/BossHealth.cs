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
        GetComponent<AudioSource>().pitch = Random.Range(1.2f, 1.5f);
        GetComponent<AudioSource>().PlayOneShot(deathSFX, deathVolume);
    }

}
