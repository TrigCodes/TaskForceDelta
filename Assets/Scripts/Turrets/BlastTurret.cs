/***************************************************************
*file: BlastTurret.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behavior for Blast Turret
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastTurret : Turret
{
    [Header("Blast Turret Specific Attributes")]
    [SerializeField] private float blastRadius = 2f; // The radius of the explosion

    [Header("Special Upgrade Attributes")]
    [SerializeField] private int specialUpgradeCost = 100;
    [SerializeField] private float upgradeBlastRadius = 5f; // New blast radius with upgrade

    private bool specialUpgradeDone = false;
    // function: Shoot
    // purpose: shoot at target position with explosive bullet
    protected override void Shoot(Vector3 targetPosition, bool canSeeStealthEnemies)
    {
        base.Shoot(targetPosition, canSeeStealthEnemies);

        // Prepare bullet object and shoot
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        ExplosiveBullet explosiveBulletScript = bullet.GetComponent<ExplosiveBullet>();
        if (explosiveBulletScript != null)
        {
            explosiveBulletScript.SetDirection((targetPosition - firingPoint.position).normalized);
            explosiveBulletScript.SetDamage(damage); // Set damage for individual enemies hit by the blast
            explosiveBulletScript.SetBlastRadius(blastRadius);
            // Target enemies
            if (canSeeStealthEnemies)
            {
                explosiveBulletScript.SetTargetTags(new List<string> { "Enemy", "StealthEnemy" });
            }
            else
            {
                explosiveBulletScript.SetTargetTags(new List<string> { "Enemy" });
            }
            // For collateral damage
            explosiveBulletScript.SetCollateralTags(new List<string> {"Enemy", "StealthEnemy" });
        }
    }
    // function: UpgradeSpecial
    // purpose: Increase blast radius
    public override bool UpgradeSpecial()
    {
        if (!specialUpgradeDone && LevelManager.main.SpendScraps(specialUpgradeCost))
        {
            blastRadius = upgradeBlastRadius;
            specialUpgradeDone = true;
            return true;
        }
        else
        {
            FindObjectOfType<Alert>().DisplayAlert($"Not Enough Scraps for Upgrade");
            return false;
        }
    }
    // function: GetSpecialInfoText
    // purpose: return string when special upgrade
    public override string GetSpecialInfoText()
    {
        return "Increase Blast Radius";
    }
    // function: GetSpecialUpgradeDone
    // purpose: true if special upgrade is done
    public override bool GetSpecialUpgradeDone()
    {
        return specialUpgradeDone;
    }
    // function: GetSpecialUpgradeCost
    // purpose: get cost of special upgrade
    public override int GetSpecialUpgradeCost()
    {
        return specialUpgradeCost;
    }
}