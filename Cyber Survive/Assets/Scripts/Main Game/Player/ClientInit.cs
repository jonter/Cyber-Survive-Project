using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ClientInit : MonoBehaviour
{
    [SerializeField] GameObject ghostPrefab;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(ghostPrefab.name, new Vector3(), Quaternion.identity);
    }

   
}
