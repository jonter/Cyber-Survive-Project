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

    [Header("Audio Settings")]
    [SerializeField] protected AudioClip skillSFX;
    [Range(0f, 1f)] [SerializeField] protected float skillVolume = 1;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        view = GetComponent<PhotonView>();
        charMove = GetComponent<CharacterMovement>();
        playerShoot = GetComponent<SoldierShoot>();
        if (view.IsMine)
        {
            display.ChangeIcon(skillIcon);
            SetLevel();
        }
    }

    void SetLevel()
    {
        int level = PlayerPrefs.GetInt("trooper");
        duration = 5 + level * 0.5f;
        if (duration > 12) duration = 12;
        if(skillVFX != null) view.RPC("SetSkillVFX", RpcTarget.All, duration);
    }

    [PunRPC]
    void SetSkillVFX(float dur)
    {

        ParticleSystem.MainModule main = skillVFX.main;
        main.duration = dur;
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
        GetComponent<AudioSource>().PlayOneShot(skillSFX, skillVolume);
    }

}
