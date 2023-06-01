using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCamera : MonoBehaviour
{
    PhotonView playerView;
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        playerView = GetComponentInParent<PhotonView>();
        transform.parent = null;
        offset = transform.position - playerView.transform.position;

        if(playerView.IsMine == false)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerView == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = playerView.transform.position + offset;
        }
        
    }
}
