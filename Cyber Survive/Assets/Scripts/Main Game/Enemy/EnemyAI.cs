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
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (PhotonNetwork.IsMasterClient == false) return;
        StartCoroutine(ChoosePlayerToChase());
    }

    IEnumerator ChoosePlayerToChase()
    {
        yield return new WaitForSeconds(1);
        characters = FindObjectsOfType<CharacterMovement>();
        if(characters.Length > 0)
        {
            // найти ближайшего игрока
            target = characters[0].transform;
        }

        StartCoroutine(ChoosePlayerToChase());
    }


    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        if (target == null) return;

        agent.SetDestination(target.position);
    }
}
