using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TechesSkill : SoldierSkill
{
    float damage = 10;
    float fireRate = 1.5f;
    [SerializeField] GameObject spawnTurrelZone;
    [SerializeField] GameObject turrelPrefab;

    bool isActivated = false;
    [SerializeField] LayerMask collideTurrelLayers;

    protected override void Start()
    {
        base.Start();
        if (view.IsMine == false) return;
        int level = PlayerPrefs.GetInt("teches");
        damage = 10 + level;
        fireRate = 1.5f + level * 0.1f;
        duration = 8 * level;
        if(duration > 18) duration = 18; 
    } 

    protected override void Update()
    {
        if (view.IsMine == false) return;
        if (isReloaded == false) return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            isActivated = !isActivated;
            if (isActivated) ActivateBuild();
            else DeactivateBuild();
        }

        if (isActivated == true) SetTurrel();
        
    }


    void SetTurrel()
    {
        Vector3 center = spawnTurrelZone.transform.position;
        bool canSpawn = Physics.CheckSphere(center, 1, collideTurrelLayers);
        if(canSpawn == true)
        {// есть препятствие
            Material m = spawnTurrelZone.GetComponent<MeshRenderer>().material;
            Color c = Color.red;
            c.a = 0.15f;
            m.color = c;
        }
        else
        {// нет препятствия
            Material m = spawnTurrelZone.GetComponent<MeshRenderer>().material;
            Color c = Color.green;
            c.a = 0.15f;
            m.color = c;
            CreateTurrel();
        }
    }

    void CreateTurrel()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StartCoroutine(CreateCoroutine());
        }
    }

    IEnumerator CreateCoroutine()
    {
        isReloaded = false;
        isActivated = false;
        DeactivateBuild();
        Vector3 spawnPos = spawnTurrelZone.transform.position;
        Quaternion spawnRot = spawnTurrelZone.transform.rotation;
        GameObject turrel = 
            PhotonNetwork.Instantiate(turrelPrefab.name, spawnPos, spawnRot);
        Turrel t = turrel.GetComponent<Turrel>();
        t.Setup(damage, fireRate, 10);

        display.Reload(reloadTime);
        yield return new WaitForSeconds(reloadTime);
        isReloaded = true;
    }


    void ActivateBuild()
    {
        spawnTurrelZone.SetActive(true);
        playerShoot.enabled = false;
    }

    void DeactivateBuild()
    {
        spawnTurrelZone.SetActive(false);
        playerShoot.enabled = true;
    }

}
