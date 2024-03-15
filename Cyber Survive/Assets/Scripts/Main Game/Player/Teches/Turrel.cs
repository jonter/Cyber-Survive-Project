using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class Turrel : MonoBehaviour
{
    [SerializeField] float searchRadius = 10;
    LineRenderer shootLine;

    float damage = 10;
    float fireRate = 1;

    bool isReloaded = true;

    EnemyHealth target;
    PhotonView view;

    [SerializeField] Transform topPart;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        topPart.localPosition = new Vector3(0, 50, 0);
        topPart.DOLocalMoveY(0.5f, 0.5f).SetEase(Ease.InSine);
        view = GetComponent<PhotonView>();
        shootLine = GetComponentInChildren<LineRenderer>();
        if (view.IsMine == false) return;
        StartCoroutine(SearchEnemy());
    }

    private void OnDestroy()
    {
        topPart.DOKill();
        transform.DOKill();
    }

    public void Setup(float damage, float fireRate, float duration)
    {
        this.damage = damage;
        this.fireRate = fireRate;
        StartCoroutine(DissolveTurrel(duration));
    }

    IEnumerator DissolveTurrel(float duration)
    {
        yield return new WaitForSeconds(duration);
        view.RPC("HideTurrel", RpcTarget.All);
        yield return new WaitForSeconds(0.6f);
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void HideTurrel()
    {
        topPart.DOLocalMoveY(60f, 0.5f).SetEase(Ease.InSine);
        enabled = false;
        transform.DOMoveY(-2, 0.5f).SetDelay(0.1f);
        Vector3 rot = new Vector3(0, 180, 0);
        transform.DORotate(rot, 0.5f, RotateMode.WorldAxisAdd).SetDelay(0.1f);
    }

 

    IEnumerator SearchEnemy()
    {
        yield return new WaitForSeconds(1);

        if (target == null)
        {
            LayerMask enemyLayer = LayerMask.GetMask("Enemy");
            Collider[] enemies = Physics.OverlapSphere(transform.position, searchRadius, enemyLayer);
            if (enemies.Length > 0)
            {
                target = FindTarget(enemies);
            }
        }

        StartCoroutine(SearchEnemy());
    }

    EnemyHealth FindTarget(Collider[] enemies)
    {
        EnemyHealth enemy = enemies[0].GetComponent<EnemyHealth>();
        float min = 20;

        for (int i = 0; i < enemies.Length; i++)
        {
            EnemyHealth e = enemies[i].GetComponent<EnemyHealth>();
            if (e.GetAlive() == false) continue;
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (distance < min)
            {
                enemy = e;
                min = distance;
            }
        }
        return enemy;
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine == false) return;
        if (target == null) return;
        LookAtTarget();
        CheckEnemy();

        if (isReloaded == true) StartCoroutine(ShootCoroutine());

    }

    IEnumerator ShootCoroutine()
    {
        isReloaded = false;
        DamageTarget();
        view.RPC("DrawLine", RpcTarget.All, target.transform.position);
        yield return new WaitForSeconds(1 / fireRate);
        isReloaded = true;
    }

    void DamageTarget()
    {
        PhotonView tView = target.GetComponent<PhotonView>();
        tView.RPC("GetDamage", RpcTarget.MasterClient, damage, view.Owner.NickName);
    }

    [PunRPC]
    void DrawLine(Vector3 endPos)
    {
        StartCoroutine(DrawLineCoroutine(endPos));
    }
    IEnumerator DrawLineCoroutine(Vector3 endPos)
    {
        shootLine.enabled = true;
        shootLine.SetPosition(0, shootLine.transform.position);
        shootLine.SetPosition(1, endPos);
        yield return new WaitForSeconds(0.1f);
        shootLine.enabled = false;
    }


    void CheckEnemy()
    {
        if (target.GetAlive() == false) target = null;
    }

    void LookAtTarget()
    {
        Vector3 dir = target.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
        transform.rotation = rot;
    }


    public IEnumerator DestroyTurrel(float duration)
    {
        yield return new WaitForSeconds(duration);  
        PhotonNetwork.Destroy(gameObject);
    }

}
