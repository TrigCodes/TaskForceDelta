using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerEnemy : Enemy
{
    [Header("Ranger Specific Attributes")]
    [SerializeField] private GameObject bulletPrefab; // The bullet prefab to shoot
    [SerializeField] private Transform firingPoint; // The point from which bullets are fired
    [SerializeField] private float shootingRange = 5f; // Maximum shooting range
    [SerializeField] private float fireRate = 1f; // Bullets per second

    private float timeSinceLastShot = 0; // Time counter to control shooting rate

    protected override void Update()
    {
        base.Update();

        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) <= shootingRange)
            {
                MoveTowardsTarget(false); // Stop moving when in range
                Shoot();
            }
            else
            {
                MoveTowardsTarget(true); // Continue moving towards the target
            }
        }
    }

    protected void Shoot()
    {
        if (timeSinceLastShot >= 1f / fireRate)
        {
            // Prepare bullet object and shoot
            GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection((target.position - firingPoint.position).normalized);
                bulletScript.SetDamage(damage);
                // Target enemies
                bulletScript.SetTargetTags(new List<string> { "Turret", "Core" });
            }
            timeSinceLastShot = 0;
        }
        else
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }
}