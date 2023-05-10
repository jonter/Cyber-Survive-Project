using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;
    PhotonView view;
    [SerializeField] ParticleSystem fireVFX;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine == false) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 spawnPos = shootPoint.position;
            GameObject newBullet = PhotonNetwork.Instantiate(bulletPrefab.name,
                spawnPos, transform.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = transform.forward * 10;
            view.RPC("PlayFireEffect", RpcTarget.All);
        }
        
    }

    [PunRPC]
    void PlayFireEffect()
    {
        fireVFX.Play();
    }

}
