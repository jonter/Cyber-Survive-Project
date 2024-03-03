using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrel : MonoBehaviour
{
    [SerializeField] float searchRadius = 10;
    LineRenderer shootLine;

    float damage = 10;
    float fireRate = 1;

    bool isReloaded = true;

    EnemyHealth target;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        shootLine = GetComponentInChildren<LineRenderer>();
        StartCoroutine(SearchEnemy());
    }

    IEnumerator SearchEnemy()
    {
        yield return new WaitForSeconds(1);

        if(target == null)
        {
            LayerMask enemyLayer = LayerMask.GetMask("Enemy");
            Collider[] enemies = Physics.OverlapSphere(transform.position, searchRadius, enemyLayer);
            if(enemies.Length > 0)
            {
                target = enemies[0].GetComponent<EnemyHealth>();
            }
        }

        StartCoroutine(SearchEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        LookAtTarget();
        CheckEnemy();

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

}
