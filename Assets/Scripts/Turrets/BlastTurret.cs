using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastTurret : Turret
{
    [Header("Blast Turret Specific Attributes")]
    [SerializeField] private float blastRadius = 3f; // The radius of the explosion

    protected override void Shoot(Vector3 targetPosition)
    {
        // Prepare bullet object and shoot
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        ExplosiveBullet explosiveBullet = bullet.GetComponent<ExplosiveBullet>();
        if (explosiveBullet != null)
        {
            explosiveBullet.SetDirection((targetPosition - firingPoint.position).normalized);
            explosiveBullet.SetDamage(damage); // Set damage for individual enemies hit by the blast
            explosiveBullet.SetBlastRadius(blastRadius);
            // Target enemies
            explosiveBullet.SetTargetTags(new List<string> { "Enemy"});
            // For collateral damage
            explosiveBullet.SetCollateralTags(new List<string> {"Enemy", "StealthEnemy" });
        }
    }
}