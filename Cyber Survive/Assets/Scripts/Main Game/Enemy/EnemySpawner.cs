using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject robotPrefab;
    [SerializeField] GameObject dronePrefab;
    [SerializeField] GameObject kamikadzePrefab;
    [SerializeField] GameObject lizardBossPrefab;
    EnemySpawnPoint[] spawnPoints;

    public int wave = 0;
    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient == false) return;
        spawnPoints = FindObjectsOfType<EnemySpawnPoint>();
        
    }

    public void StartSpawning()
    {
        if (wave > 0) return;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        wave++;
        view.RPC("SetWaveRPC", RpcTarget.Others, wave);
        GameManager.master.DisplayWave(wave);
        int enemyCount = 3 + wave + Random.Range(0, 2);
        if(enemyCount > 20) enemyCount = 20 + Random.Range(0, wave);
      
        yield return StartCoroutine(SpawnEnemiesInWave(enemyCount));
        yield return StartCoroutine(WaitForEnemyDeath());
        yield return new WaitForSeconds(5);

        StartCoroutine(SpawnWave());
    }

    [PunRPC]
    void SetWaveRPC(int w)
    {
        wave = w;
    }

    IEnumerator WaitForEnemyDeath()
    {
        for (int i = 1; i <= 50; i++)
        {
            yield return new WaitForSeconds(1);
            // Обновить текст со временем волны
            EnemyHealth[] enemies = FindObjectsOfType<EnemyHealth>();
            if (enemies.Length <= 0) yield break;
        }

    }

    IEnumerator SpawnEnemiesInWave( int count)
    {
        int rand = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPos = spawnPoints[rand].transform.position;
        for (int i = 0; i < count; i++)
        {
            GameObject ePrefab = ChooseEnemyPrefab();
            Vector3 randVec = Random.insideUnitSphere * 2;
            randVec.y = 0;
            randVec += spawnPos;
            GameObject clone = PhotonNetwork.Instantiate(ePrefab.name, randVec, Quaternion.identity);
            clone.GetComponent<EnemyHealth>().IncreaseHP(1 + wave * 0.1f );
            yield return new WaitForSeconds(0.2f);
        }
        SpawnBoss(spawnPos);
    }

    void SpawnBoss(Vector3 spawnPos)
    {
        if(wave > 0 && wave % 5 == 0)
        {
            GameObject clone = PhotonNetwork.Instantiate
                (lizardBossPrefab.name, spawnPos, Quaternion.identity);
            clone.GetComponent<EnemyHealth>().IncreaseHP(1 + wave * 0.1f);
        }

    }

    GameObject ChooseEnemyPrefab()
    {

        float rand = Random.Range(0f, wave);
        if (rand >= 10) rand = Random.Range(0f, 10f);
        
        if (rand <= 3) return robotPrefab;
        else if (rand <= 6) return dronePrefab;
;       
        return kamikadzePrefab;

    }

    

    
}
