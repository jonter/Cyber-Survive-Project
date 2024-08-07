using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 6;
    Rigidbody rb;
    PhotonView view;

    [SerializeField] Animator anim;
    [SerializeField] float animspeed = 0.9f;

    public IEnumerator IncreseSpeedCoroutine(float duration)
    {
        maxSpeed *= 1.4f;
        animspeed *= 1.3f;
        anim.SetFloat("animspeed", animspeed);
        yield return new WaitForSeconds(duration);
        maxSpeed /= 1.4f;
        animspeed /= 1.3f;
        anim.SetFloat("animspeed", animspeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        anim.SetFloat("animspeed", animspeed);
    }

    private void OnDisable()
    {
        rb.velocity = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine == false) return;

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(inputX, 0, inputZ);
        if (dir.magnitude > 1) dir = dir.normalized;

        rb.velocity = dir * maxSpeed;
        anim.SetFloat("speed", dir.magnitude);
        
    }

    
}
