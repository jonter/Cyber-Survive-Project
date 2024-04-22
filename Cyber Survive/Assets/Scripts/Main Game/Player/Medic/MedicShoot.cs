using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MedicShoot : SoldierShoot
{
    float bulletCount = 20;

    protected override void Start()
    {
        fireRate = 1;
        damage = 1.5f;
        view = GetComponent<PhotonView>();
        if (view.IsMine == false) return;
        int level = PlayerPrefs.GetInt("medic");
        damage = 1.5f + level * 0.2f;

    }

    protected override void Update()
    {
        if (CanShoot() == false) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            view.RPC("PlayFireEffect", RpcTarget.All);
            MakeRaycasts();
            StartCoroutine(BusyWeapon());
        }

    }

    void MakeRaycasts()
    {
        LayerMask layer = LayerMask.GetMask("Default", "Env", "Enemy");
        for (int i = 1; i <= bulletCount; i++)
        {
            float angle = Random.Range(-i * 0.75f, i * 0.75f);
            Quaternion randomRot = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 dir = randomRot * transform.forward;
            RaycastHit hitInfo;
            Physics.Raycast(shootPoint.position, dir, out hitInfo, 10, layer);
            Debug.DrawRay(shootPoint.position, dir, Color.red, 1);
            if (hitInfo.transform == null) return;
            EnemyHealth enemy = hitInfo.transform.GetComponent<EnemyHealth>();
            if(enemy == null) return;
            PhotonView enemyView = enemy.GetComponent<PhotonView>();
            enemyView.RPC("GetDamage", RpcTarget.MasterClient, damage, view.Owner.NickName);

        }
    }

}
