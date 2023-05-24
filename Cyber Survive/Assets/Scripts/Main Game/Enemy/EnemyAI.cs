using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyAI : MonoBehaviour
{
    CharacterMovement[] characters;
    NavMeshAgent agent;

    Transform target;
    [SerializeField] float attackDistance = 1.5f;

    bool isAction = false;
    Animator anim;
    float rotationSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        if (PhotonNetwork.IsMasterClient == false) return;
        StartCoroutine(ChoosePlayerToChase());
    }


    IEnumerator ChoosePlayerToChase()
    {
        yield return new WaitForSeconds(1);
        characters = FindObjectsOfType<CharacterMovement>();
        if(characters.Length > 0)
        {
            target = SelectClosePlayer();
        }

        StartCoroutine(ChoosePlayerToChase());
    }

    Transform SelectClosePlayer()
    {
        Transform closePlayer = characters[0].transform;
        float minDistance = Vector3.Distance(transform.position, closePlayer.position);

        for (int i = 1; i < characters.Length; i++)
        {
            float distance = Vector3.Distance(transform.position,
                characters[i].transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                closePlayer = characters[i].transform;
            }

        }

        return closePlayer;
    }


    // Update is called once per frame
    void Update()
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
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackDistance)
        {
            PhotonView playerView = target.GetComponent<PhotonView>();
            playerView.RPC("GetDamage", playerView.Owner, 20f);
        }
        yield return new WaitForSeconds(19f/ 30f);

        isAction = false;
    }

    IEnumerator RotateToPlayer()
    {
        float timer = 0;
        while(timer <= 1)
        {
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
