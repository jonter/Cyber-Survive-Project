using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechesShoot : SoldierShoot
{
    [SerializeField] Camera cam;
     
    protected override void Start()
    {
        fireRate = 0.5f;
        damage = 20;
        view = GetComponent<PhotonView>();
    }

    protected override void Update()
    {
        if (CanShoot() == false) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartCoroutine(BusyWeapon());
            view.RPC("PlayFireEffect", RpcTarget.All);
            MakeShoot();
        }

    }

    void MakeShoot()
    {
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        LayerMask aimLayer = LayerMask.GetMask("Aim");
        RaycastHit hitInfo;
        Physics.Raycast(r, out hitInfo, 100, aimLayer);
        if (hitInfo.transform == null) return;
        float distance = Vector3.Distance(transform.position, hitInfo.point);
        distance = Mathf.Clamp(distance, 4, 13);

        Vector3 dir = transform.position + transform.forward * distance;
        dir.y = 0;
        GameObject grenade = PhotonNetwork.Instantiate(projectilePrefab.name,
            shootPoint.position, shootPoint.rotation);
        float flyTime = 0.6f + distance * 0.05f;
        grenade.GetComponent<Grenade>().Launch(dir, damage, flyTime);
    }

}
