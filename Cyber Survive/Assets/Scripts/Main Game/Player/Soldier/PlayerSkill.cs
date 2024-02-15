using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Stopped here...
public class PlayerSkill : MonoBehaviour
{
    [SerializeField] ParticleSystem skillVFX;
    [HideInInspector] public SkillDisplay display;

    float duration = 5;
    float reloadTime = 30;

    CharacterMovement charMove;
    SoldierShoot playerShoot;

    bool isReloaded = true;
    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        charMove = GetComponent<CharacterMovement>();
        playerShoot = GetComponent<SoldierShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine == false) return;
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(ActivateSkill());  
        }
    }


    IEnumerator ActivateSkill()
    {
        if(isReloaded == false) yield break;
        isReloaded = false;
        view.RPC("ActivateVFX", RpcTarget.All);
        StartCoroutine(playerShoot.IncreseFireCoroutine(duration));
        StartCoroutine(charMove.IncreseSpeedCoroutine(duration));
        display.Reload(reloadTime);
        yield return new WaitForSeconds(reloadTime);
        isReloaded = true;
    }

    [PunRPC]
    void ActivateVFX()
    {
        skillVFX.Play();
    }

}
