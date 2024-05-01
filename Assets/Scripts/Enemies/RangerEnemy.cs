using System;
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

                // Set target tags based on current target type
                if (target.CompareTag("Wall"))
                {
                    bulletScript.SetTargetTags(new List<string> { "Turret", "Core", "Wall" });
                }
                else
                {
                    bulletScript.SetTargetTags(new List<string> { "Turret", "Core" });
                }
            }
            timeSinceLastShot = 0;
        }
        else
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    protected override void GetClosestTarget()
    {
        // Get all possible targets in the scene
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        GameObject[] cores = GameObject.FindGameObjectsWithTag("Core");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        Transform closestTurretOrCore = null;
        float closestTurretOrCoreDistance = Mathf.Infinity;
        Transform closestWall = null;
        float closestWallDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        // Find closest Turret or Core
        foreach (GameObject turret in turrets)
        {
            float distance = Vector3.Distance(currentPosition, turret.transform.position);
            if (distance < closestTurretOrCoreDistance)
            {
                closestTurretOrCoreDistance = distance;
                closestTurretOrCore = turret.transform;
            }
        }
        foreach (GameObject core in cores)
        {
            float distance = Vector3.Distance(currentPosition, core.transform.position);
            if (distance < closestTurretOrCoreDistance)
            {
                closestTurretOrCoreDistance = distance;
                closestTurretOrCore = core.transform;
            }
        }

        // Find closest Wall
        foreach (GameObject wall in walls)
        {
            float distance = Vector3.Distance(currentPosition, wall.transform.position);
            if (distance < closestWallDistance)
            {
                closestWallDistance = distance;
                closestWall = wall.transform;
            }
        }

        // Decide target based on distances and shooting range
        if (closestWall != null && closestTurretOrCore != null)
        {
            float difference = Vector3.Distance(closestTurretOrCore.position, closestWall.position);
            
            if (difference <= shootingRange)
            {
                target = closestTurretOrCore;
            }
            else if (closestWall != null)
            {
                target = closestWall;
            }
        }
        else
        {
            target = closestTurretOrCore;
        }
    }
}