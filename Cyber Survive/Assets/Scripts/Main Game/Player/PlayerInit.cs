using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInit : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 randomVec = Random.insideUnitSphere * 5;
        Vector3 spawnPos = new Vector3(randomVec.x, 1, randomVec.z);
        PhotonNetwork.Instantiate(characterPrefab.name, spawnPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
