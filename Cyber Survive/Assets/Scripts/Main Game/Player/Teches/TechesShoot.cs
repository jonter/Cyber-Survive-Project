using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechesShoot : SoldierShoot
{
     
    protected override void Start()
    {
        fireRate = 0.5f;
        damage = 30;
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
        Vector3 dir = transform.position + transform.forward * 7;
        dir.y = 0;
        GameObject grenade = PhotonNetwork.Instantiate(projectilePrefab.name,
            shootPoint.position, shootPoint.rotation);
        grenade.GetComponent<Grenade>().Launch(dir, damage, 1);


    }

}
