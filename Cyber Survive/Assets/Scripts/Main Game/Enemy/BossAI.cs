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
    [SerializeField] ParticleSystem fireVFX;
    [SerializeField] GameObject rocketPrefab;

    AudioSource audio;

    [SerializeField] AudioClip skillSFX;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }


    protected override void Start()
    {
        audio = GetComponent<AudioSource>();
        if (PhotonNetwork.IsMasterClient == false) return;
        base.Start();
        GetComponentInChildren<BossDamageTrigger>().damage = damage;
        damageTrigger = GetComponentInChildren<BossDamageTrigger>()
            .GetComponent<BoxCollider>();

        damageTrigger.enabled = false;
        rotationSpeed = 15;
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
            StartCoroutine(MakeSkillCoroutine());
            timeBeforeAbility = 0;
        }
        else if (distance <= attackDistance)
        {
            StartCoroutine(AttackCoroutine());
        }
        HandleMoveAnim();
    }

    IEnumerator MakeSkillCoroutine()
    {
        agent.SetDestination(transform.position);
        isAction = true;
        view.RPC("MakeStompAnim", RpcTarget.All);
        yield return new WaitForSeconds(25f / 30f);
        SpawnBombsAroundBoss();
        StartCoroutine(SpawnBombsAtPlayers());
        yield return new WaitForSeconds(35f / 30f);
        isAction = false;
    }

    [PunRPC]
    void MakeStompAnim()
    {
        anim.SetTrigger("stomp");
        StartCoroutine(PlaySkillSound());
    }

    IEnumerator PlaySkillSound()
    {
        yield return new WaitForSeconds(20f / 30f);
        audio.PlayOneShot(skillSFX);
    }

    IEnumerator SpawnBombsAtPlayers()
    {
        PlayerHealth[] players = FindObjectsOfType<PlayerHealth>();
        foreach (PlayerHealth p in players)
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 pos = p.transform.position + Random.insideUnitSphere * 3;
            pos.y = 0;
            PhotonNetwork.Instantiate(rocketPrefab.name, pos, Quaternion.identity);
            if(Random.Range(0, 2) == 0)
            {
                pos = p.transform.position + Random.insideUnitSphere * 2;
                pos.y = 0;
                PhotonNetwork.Instantiate(rocketPrefab.name, pos, Quaternion.identity);
            }
        }

    }

    void SpawnBombsAroundBoss()
    {
        Vector3 pos1 = transform.position + new Vector3(3,0,0);
        Vector3 pos2 = transform.position + new Vector3(-3,0,0);
        Vector3 pos3 = transform.position + new Vector3(0,0,3);
        Vector3 pos4 = transform.position + new Vector3(0,0,-3);

        PhotonNetwork.Instantiate(rocketPrefab.name, pos1, Quaternion.identity);
        PhotonNetwork.Instantiate(rocketPrefab.name, pos2, Quaternion.identity);
        PhotonNetwork.Instantiate(rocketPrefab.name, pos3, Quaternion.identity);
        PhotonNetwork.Instantiate(rocketPrefab.name, pos4, Quaternion.identity);
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
        Vector3 startRot = new Vector3(0, -60, 0);// probably decrease rotation
        transform.DORotate(startRot, 0.3f, RotateMode.WorldAxisAdd);
        yield return null;
        anim.SetBool("shoot", true);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(ShootCoroutine());
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(RotateToPlayer(0.4f));
        yield return new WaitForSeconds(0.4f);
        isAction = false;
    }

    IEnumerator ShootCoroutine()
    {
        view.RPC("PlayFireEffect", RpcTarget.All);
        yield return new WaitForSeconds(0.1f);
        damageTrigger.enabled = true;
        Vector3 rot = new Vector3(0, 120, 0);
        transform.DORotate(rot, 1.2f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(1.2f);
       
        damageTrigger.enabled = false;
        view.RPC("StopFireEffect", RpcTarget.All);
    }

    [PunRPC]
    void PlayFireEffect()
    {
        fireVFX.Play();
        audio.Play();
    }

    [PunRPC]
    void StopFireEffect()
    {
        fireVFX.Stop();
        audio.Stop();
    }




}
