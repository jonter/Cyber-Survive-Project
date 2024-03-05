using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SoldierSkill : MonoBehaviour
{
    [SerializeField] ParticleSystem skillVFX;
    [HideInInspector] public SkillDisplay display;

    protected float duration = 5;
    protected float reloadTime = 30;

    CharacterMovement charMove;
    protected SoldierShoot playerShoot;

    protected bool isReloaded = true;
    protected PhotonView view;

    [SerializeField] Sprite skillIcon;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        view = GetComponent<PhotonView>();
        charMove = GetComponent<CharacterMovement>();
        playerShoot = GetComponent<SoldierShoot>();
        if(view.IsMine) display.ChangeIcon(skillIcon);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (view.IsMine == false) return;
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(ActivateSkill());  
        }
    }


    protected virtual IEnumerator ActivateSkill()
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
