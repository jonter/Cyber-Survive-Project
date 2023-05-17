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
    // Start is called before the first frame update
    void Start()
    {
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

    }

    IEnumerator MakeHit()
    {
        isAction = true;
        // ¬ключаем анимацию
        yield return new WaitForSeconds(0.5f);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackDistance)
        {
            PhotonView playerView = target.GetComponent<PhotonView>();
            playerView.RPC("GetDamage", playerView.Owner, 20f);

        }

        isAction = false;
    }

}
