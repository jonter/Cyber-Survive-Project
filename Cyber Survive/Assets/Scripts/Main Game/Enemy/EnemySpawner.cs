using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    EnemySpawnPoint[] spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient == false) return;
        spawnPoints = FindObjectsOfType<EnemySpawnPoint>();
        StartCoroutine(SpawnNewEnemy());
    }

    IEnumerator SpawnNewEnemy()
    {
        yield return new WaitForSeconds(2);
        int r = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPos = spawnPoints[r].transform.position;

        PhotonNetwork.Instantiate(enemyPrefab.name, spawnPos, Quaternion.identity);
        StartCoroutine(SpawnNewEnemy());
    }

    
}
