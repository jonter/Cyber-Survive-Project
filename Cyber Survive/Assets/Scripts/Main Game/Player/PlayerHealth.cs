using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

public class PlayerHealth : MonoBehaviour
{
    float maxHp = 100;
    float hp;

    [SerializeField] Slider healthBar;
    PhotonView view;

    [SerializeField] Animator playerAnim;
    RigBuilder playerRig;

    [SerializeField] Rigidbody[] rigidbodies;
    [HideInInspector] public bool isAlive = true;

    [Header("Audio Settings")]
    [SerializeField] AudioClip deathSFX;

    [Range(0f, 1f)]
    [SerializeField] float deathVolume = 1;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        SetupHealth();
        playerRig = GetComponentInChildren<RigBuilder>();
        hp = maxHp;
        healthBar.value = hp / maxHp;
        SwitchRagdoll(false);
    }

    void SetupHealth()
    {
        if (view.IsMine == false) return;
        SoldierSkill charSkill = GetComponent<SoldierSkill>();
        string ID = "trooper";
        if (charSkill is MedicSkill) ID = "medic";
        if (charSkill is TechesSkill) ID = "teches";

        int level = PlayerPrefs.GetInt(ID);

        maxHp = 100 + level * 20;
        hp = maxHp;
    }

    

    void SwitchRagdoll(bool isOn)
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = !isOn;
            rigidbodies[i].GetComponent<Collider>().enabled = isOn;
        }
        playerRig.enabled = !isOn;
        playerAnim.enabled = !isOn;

    }


    [PunRPC]
    public void GetDamage(float damage)
    {
        if (isAlive == false) return;
        hp -= damage;
        view.RPC("DisplayHealth", RpcTarget.All, hp/maxHp);

        if(hp <= 0.001f)
        {
            StartCoroutine(KillPlayer());
        }
    }

    [PunRPC]
    public void RestoreHealth(float restore)
    {
        if (isAlive == false) return;
        hp += restore;
        if (hp > maxHp) hp = maxHp;
        view.RPC("DisplayHealth", RpcTarget.All, hp / maxHp);
    }


    IEnumerator KillPlayer()
    {
        isAlive = false;
        GetComponent<CharacterMovement>().enabled = false;
        GetComponent<PlayerAim>().enabled = false;
        GetComponent<SoldierShoot>().enabled = false;
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void DisplayHealth(float percent)
    {
        healthBar.value =  percent;
        if(percent <= 0.0001f)
        {
            Destroy(healthBar.gameObject);
            SwitchRagdoll(true);
            isAlive = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<AudioSource>().PlayOneShot(deathSFX, deathVolume);
        }
    }



   
}
