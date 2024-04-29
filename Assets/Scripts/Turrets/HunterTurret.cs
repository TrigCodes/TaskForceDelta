using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterTurret : Turret
{
    protected override void Update()
    {
        base.Update();
        CheckForStealthEnemies();
    }

    private void CheckForStealthEnemies()
    {
        Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (var entity in entities)
        {
            if (entity.CompareTag("StealthEnemy"))
            {
                StealthEnemy enemy = entity.GetComponent<StealthEnemy>();
                if (enemy != null)
                {
                    enemy.EnableVisibility();
                }
            }
        }
    }

    protected override void TargetAndShoot()
    {
        if (this != playerControlledTurret)
        {
            // Get all entities within view of the turret
            Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, range);
            Transform closestEnemy = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            // Find closest enemy if one exists
            foreach (Collider2D entity in entities)
            {
                // Overriden to include StealthEnemy for targeting
                if (entity.gameObject.CompareTag("Enemy") || 
                    entity.gameObject.CompareTag("StealthEnemy"))
                {
                    Vector3 directionToTarget = entity.transform.position - currentPosition;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        closestEnemy = entity.transform;
                    }
                }
            }

            if (closestEnemy != null && timeSinceLastShot >= 1f / fireRate)
            {
                Shoot(closestEnemy.position);
                timeSinceLastShot = 0;
            }
            else
            {
                timeSinceLastShot += Time.deltaTime;
            }
        }
    }

    protected override void Shoot(Vector3 targetPosition)
    {
        // Prepare bullet object and shoot
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript)
        {
            bulletScript.SetDirection((targetPosition - firingPoint.position).normalized);
            bulletScript.SetDamage(damage);
            // Target enemies
            bulletScript.SetTargetTags(new List<string> { "Enemy", "StealthEnemy" });
        }
    }
}