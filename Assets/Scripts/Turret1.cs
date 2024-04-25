using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret1 : MonoBehaviour
{
    [SerializeField] private float range = 10f;
    [SerializeField] private GameObject bullet;
    private Transform target;
    public TurretStat stat;
    private float fireCountdown = 0.0f;
    bool canShoot = true;
    private float dmgCooldown;
    void Start()
    {
        // cost, hitpoint, shield, damage, fireRate
        stat = new TurretStat(30, 100, 30, 100, 2f);
        dmgCooldown = 0.0f;

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
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
            fireCountdown = 1f/stat.FireRate;
        }
        fireCountdown -= Time.deltaTime;
    }
    // TODO: bullet spawn location needed to change.
    void Fire()
    {
        GameObject bulletSpawn = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet bulletProp = bulletSpawn.GetComponent<Bullet>();

        if (bullet != null) {
            bulletProp.Damage = stat.Damage;
            bulletProp.SetTarget(target);
        }

        // tpmBullet.GetComponent<Bullet>().Damage = stat.Damage;
        // tpmBullet.GetComponent<Bullet>().SetTarget(target);
    }

    IEnumerator AllowToShoot()
    {
        yield return new WaitForSeconds(stat.FireRate);
        canShoot = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
