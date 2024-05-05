/***************************************************************
*file: MachineGunTurret.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behavior for Machine Gun Turret
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunTurret : Turret
{
    [Header("Special Upgrade Attributes")]
    [SerializeField] private int specialUpgradeCost = 100;
    // Increase fire rate by specfied level and increase max level
    [SerializeField] private int upgradeFireRateLevel = 2;

    private bool specialUpgradeDone = false;

    // function: Shoot
    // purpose: shoot at target
    protected override void Shoot(Vector3 targetPosition, bool canSeeStealthEnemies)
    {
        base.Shoot(targetPosition, canSeeStealthEnemies);

        // Prepare bullet object and shoot
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript)
        {
            bulletScript.SetDirection((targetPosition - firingPoint.position).normalized);
            bulletScript.SetDamage(damage);
            // Target enemies
            if (canSeeStealthEnemies)
            {
                bulletScript.SetTargetTags(new List<string> { "Enemy", "StealthEnemy" });
            }
            else
            {
                bulletScript.SetTargetTags(new List<string> { "Enemy" });
            }
        }
    }
    // function: UpgradeSpecial
    // purpose: Increase rate and max fire rate to make it faster than the others
    public override bool UpgradeSpecial()
    {
        if (!specialUpgradeDone && LevelManager.main.SpendScraps(specialUpgradeCost))
        {
            fireRateMaxLevel += upgradeFireRateLevel;  // Increase max level by specfied levels

            // Increase current level by specfied levels
            for (int i = 0; i < upgradeFireRateLevel; i++)
            {
                fireRateLevel++;
                fireRate += incPerFireRateLevel;
            }

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
    // purpose: return string when turret special is upgraded
    public override string GetSpecialInfoText()
    {
        return $"Increase Fire Rate By {upgradeFireRateLevel} Levels";
    }
    // function: GetSpecialUpgradeDone
    // purpose: return if special upgrade is done
    public override bool GetSpecialUpgradeDone()
    {
        return specialUpgradeDone;
    }
    // function: GetSpecialUpgradeCost
    // purpose: return special upgrade cost
    public override int GetSpecialUpgradeCost()
    {
        return specialUpgradeCost;
    }
}