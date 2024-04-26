using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private Transform target;
    private float fireCountdown = 0.0f;

    [Header("Turret Stat")]
    [SerializeField] private int hitPoint = 30;
    [SerializeField] private int shield = 30;
    [SerializeField] private float range = 10f;
    [SerializeField] private int damage = 100;
    [SerializeField] private float fireRate = 1.0f;

    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }
        if (closestEnemy != null && shortestDistance <= range) {
            target = closestEnemy.transform;
        } else {
            target = null;
        }
    }
    void Update()
    {
        if (target == null) {
            return;
        }
        if (fireCountdown <= 0) {
            Fire();
            fireCountdown = 1f/fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }
    // TODO: bullet spawn location needed to change.
    void Fire()
    {
        GameObject bulletSpawn = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet bulletProp = bulletSpawn.GetComponent<Bullet>();

        if (bullet != null) {
            bulletProp.Damage = damage;
            bulletProp.SetTarget(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
