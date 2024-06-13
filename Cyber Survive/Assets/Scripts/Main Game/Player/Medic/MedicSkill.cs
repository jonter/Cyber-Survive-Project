using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MedicSkill : SoldierSkill
{
    [SerializeField] GameObject healingWardPrefab;

    protected override IEnumerator ActivateSkill()
    {
        if(isReloaded == false) yield break;
        isReloaded = false;
        view.RPC("PlaySoundRPC", RpcTarget.All);
        Vector3 wardPos = transform.position + new Vector3(0, -1, 0);
        GameObject ward = PhotonNetwork.Instantiate(healingWardPrefab.name,
            wardPos, Quaternion.identity);
        display.Reload(reloadTime);
        yield return new WaitForSeconds(duration);
        PhotonNetwork.Destroy(ward);
        yield return new WaitForSeconds(reloadTime - duration);
        isReloaded = true;
    }

    [PunRPC]
    void PlaySoundRPC()
    {
        GetComponent<AudioSource>().PlayOneShot(skillSFX, skillVolume);
    }
}
