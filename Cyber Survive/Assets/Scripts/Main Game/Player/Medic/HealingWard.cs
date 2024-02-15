using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealingWard : MonoBehaviour
{
    PhotonView view;
    PlayerHealth[] players;

    float healGap = 0.5f;
    float healingPower = 10;

    [SerializeField] float healRadius = 5;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine == false) return;
        players = FindObjectsOfType<PlayerHealth>();
        StartCoroutine(HealPlayers());
    }

    IEnumerator HealPlayers()
    {
        yield return new WaitForSeconds(healGap);
        for (int i = 0; i < players.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, players[i].transform.position);
            if(distance < healRadius)
            {
                PhotonView playerView = players[i].GetComponent<PhotonView>();
                playerView.RPC("RestoreHealth", playerView.Owner, healingPower);
            }
        }
        StartCoroutine(HealPlayers());
    }

        
}
