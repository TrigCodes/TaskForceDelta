using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    private Transform target;
    private float fireCountdown = 0.0f;

    [Header("Stat")]
    [SerializeField] private float range = 10f;
    [SerializeField] private int damage = 100;
    [SerializeField] private float fireRate = 0.3f;

    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }
    void UpdateTarget()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        float shortestDistance = Mathf.Infinity;
        GameObject closestTurret = null;
        foreach (GameObject turret in turrets)
        {
            float distanceToTurret = Vector2.Distance(transform.position, turret.transform.position);
            if (distanceToTurret < shortestDistance)
            {
                shortestDistance = distanceToTurret;
                closestTurret = turret;
            }
        }
        if (closestTurret != null && shortestDistance <= range) {
            target = closestTurret.transform;
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
            bulletProp.CanSee = true;
            bulletProp.TargetType = "Turret";
            bulletProp.SetTarget(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
