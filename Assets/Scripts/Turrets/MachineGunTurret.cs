using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunTurret : Turret
{
    protected override void Start()
    {
        // Custom variables for Machine Gun Turret
        // Overrides values when game starts
        fireRate = 5f;
        range = 5f;
        damage = 40;
        maxHealth = 200;

        base.Start();  // Call base to ensure any base initialization happens
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
            bulletScript.SetTargetTags(new List<string> { "Enemy" });
        }
    }
}
