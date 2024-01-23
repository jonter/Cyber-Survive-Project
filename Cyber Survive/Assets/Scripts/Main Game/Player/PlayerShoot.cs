using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.EventSystems;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;
    PhotonView view;
    [SerializeField] ParticleSystem fireVFX;

    float bulletSpeed = 15;

    float fireRate = 3;
    bool isAction = false;

    float damage = 10;

    public IEnumerator IncreseFireCoroutine(float duration)
    {
        fireRate *= 1.7f;
        yield return new WaitForSeconds(duration);
        fireRate /= 1.7f;
    }

    IEnumerator BusyWeapon()
    {
        isAction = true;
        yield return new WaitForSeconds(1/fireRate);
        isAction = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine == false) return;
        int level = PlayerPrefs.GetInt("level");
        damage = 10 + level * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine == false) return;
        if (isAction == true) return;
        if (EventSystem.current.IsPointerOverGameObject() == true) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartCoroutine(BusyWeapon());
            Vector3 spawnPos = shootPoint.position;
            GameObject newBullet = PhotonNetwork.Instantiate(bulletPrefab.name,
                spawnPos, transform.rotation);
            newBullet.GetComponent<Bullet>().Launch(damage, transform.forward * bulletSpeed);
            
            view.RPC("PlayFireEffect", RpcTarget.All);
        }
        
    }

    [PunRPC]
    void PlayFireEffect()
    {
        fireVFX.Play();
    }

}
