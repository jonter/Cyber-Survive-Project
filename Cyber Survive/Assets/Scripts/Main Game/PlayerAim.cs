using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAim : MonoBehaviour, IPunObservable
{
    PhotonView view;
    Camera cam;

    Vector3 aimPoint;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(view.IsMine == true)
        {// срабатывает каждый кадр на нашем персонаже
            Aim();
        }
        else
        {// делать что-то на персонаже, если это не мой персонаж

        }
        RotateToAim();

    }

    void Aim()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        float distance = 100;
        LayerMask aimLayer = LayerMask.GetMask("Aim");
        RaycastHit hitInfo;
        Physics.Raycast(r, out hitInfo, distance,  aimLayer);
        if (hitInfo.transform)
        {
            aimPoint = hitInfo.point;
        }

    }

    void RotateToAim()
    {
        transform.LookAt(aimPoint);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting == true)
        {
            stream.SendNext(aimPoint);
        }
        else
        {
            aimPoint = (Vector3) stream.ReceiveNext();
        }
        
    }
}
