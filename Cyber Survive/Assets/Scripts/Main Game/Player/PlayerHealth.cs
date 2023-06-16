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
    public bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        playerRig = GetComponentInChildren<RigBuilder>();
        hp = maxHp;
        healthBar.value = hp / maxHp;
        view = GetComponent<PhotonView>();
        SwitchRagdoll(false);
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
        view.RPC("DisplayHealth", RpcTarget.All, hp);

        if(hp <= 0.001f)
        {
            StartCoroutine(KillPlayer());
        }
    }

    IEnumerator KillPlayer()
    {
        isAlive = false;
        GetComponent<CharacterMovement>().enabled = false;
        GetComponent<PlayerAim>().enabled = false;
        GetComponent<PlayerShoot>().enabled = false;
        yield return new WaitForSeconds(5);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void DisplayHealth(float newHP)
    {
        healthBar.value =  newHP / maxHp;
        if(newHP <= 0.0001f)
        {
            Destroy(healthBar.gameObject);
            SwitchRagdoll(true);
            isAlive = false;
        }
    }



   
}
