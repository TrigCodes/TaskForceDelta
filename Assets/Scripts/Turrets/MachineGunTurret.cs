using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunTurret : Turret
{
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