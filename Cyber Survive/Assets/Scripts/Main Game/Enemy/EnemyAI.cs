using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;

    protected Transform target;
    [SerializeField] protected float attackDistance = 1.5f;

    protected bool isAction = false;
    Animator anim;
    protected float rotationSpeed = 10;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        if (PhotonNetwork.IsMasterClient == false) return;
        StartCoroutine(ChoosePlayerToChase());
    }


    protected IEnumerator ChoosePlayerToChase()
    {
        yield return new WaitForSeconds(1);
        
        target = SelectClosePlayer();

        StartCoroutine(ChoosePlayerToChase());
    }

    protected Transform SelectClosePlayer()
    {
        PlayerHealth[] characters = FindObjectsOfType<PlayerHealth>();
        if(characters.Length == 0) return null;
        Transform closePlayer = characters[0].transform;
        float minDistance = 10000;

        for (int i = 0; i < characters.Length; i++)
        {
            float distance = Vector3.Distance(transform.position,
                characters[i].transform.position);
            PlayerHealth pHealth = characters[i];
            if(distance < minDistance && pHealth.isAlive == true)
            {
                minDistance = distance;
                closePlayer = characters[i].transform;
            }

        }
        if (closePlayer.GetComponent<PlayerHealth>().isAlive == false) return null;
        return closePlayer;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        if (target == null) return;
        if (isAction == true) return;

        agent.SetDestination(target.position);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackDistance)
        {
            StartCoroutine(MakeHit());
        }
        AnimHandle();
    }

    private void OnDisable()
    {
        agent.SetDestination(transform.position);
        agent.enabled = false;
    }

    void AnimHandle()
    {
        if(agent.desiredVelocity.magnitude < 0.1f)
        {
            anim.SetBool("walk", false);
        }
        else
        {
            anim.SetBool("walk", true);
        }
    }

    IEnumerator MakeHit()
    {
        StartCoroutine(RotateToPlayer());
        isAction = true;
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(13f/30f);
        if (target == null) yield break;
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackDistance)
        {
            PhotonView playerView = target.GetComponent<PhotonView>();
            playerView.RPC("GetDamage", playerView.Owner, 20f);
        }
        yield return new WaitForSeconds(19f/ 30f);

        isAction = false;
    }

    protected IEnumerator RotateToPlayer()
    {
        float timer = 0;
        while(timer <= 1)
        {
            if (target == null) yield break;
            timer += Time.deltaTime;
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y ,0);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                lookRotation, rotationSpeed * Time.deltaTime);

            yield return null;
        }

    }

}
