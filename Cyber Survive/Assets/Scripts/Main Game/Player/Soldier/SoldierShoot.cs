using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class SoldierShoot : MonoBehaviour
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform shootPoint;
    protected PhotonView view;
    [SerializeField] protected ParticleSystem fireVFX;

    protected float bulletSpeed = 15;

    protected float fireRate = 3;
    protected bool isAction = false;

    protected float damage = 10;

    public IEnumerator IncreseFireCoroutine(float duration)
    {
        fireRate *= 1.7f;
        yield return new WaitForSeconds(duration);
        fireRate /= 1.7f;
    }

    protected IEnumerator BusyWeapon()
    {
        isAction = true;
        yield return new WaitForSeconds(1 / fireRate);
        isAction = false;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine == false) return;
        int level = PlayerPrefs.GetInt("trooper");
        damage = 10 + level * 2;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (CanShoot() == false) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartCoroutine(BusyWeapon());
            Vector3 spawnPos = shootPoint.position;
            GameObject newBullet = PhotonNetwork.Instantiate(projectilePrefab.name,
                spawnPos, transform.rotation);
            newBullet.GetComponent<Bullet>().Launch(damage, transform.forward * bulletSpeed);

            view.RPC("PlayFireEffect", RpcTarget.All);
        }

    }

    protected bool CanShoot()
    {
        if (view.IsMine == false) return false;
        if (isAction == true) return false;
        if (EventSystem.current.IsPointerOverGameObject() == true) return false;

        return true;
    }

    [PunRPC]
    protected void PlayFireEffect()
    {
        fireVFX.Play();
    }

}
