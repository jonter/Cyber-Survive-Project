using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAim : MonoBehaviour, IPunObservable
{
    PhotonView view;
    Camera cam;

    Vector3 aimPoint;

    float rotateSpeed = 10;
    Rigidbody rb;
    [SerializeField] Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(view.IsMine == true)
        {// ����������� ������ ���� �� ����� ���������
            Aim();
        }
        else
        {// ������ ���-�� �� ���������, ���� ��� �� ��� ��������

        }
        RotateToAim();
        HandleAnimation();
    }

    void HandleAnimation()
    {
        float angle = Vector3.Angle(transform.forward, rb.velocity);
        float dot = Vector3.Dot(transform.right, rb.velocity);
        if (dot < 0) angle = -angle;
        anim.SetFloat("angle", angle);
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
        Vector3 dir = aimPoint - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = 
            Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

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
