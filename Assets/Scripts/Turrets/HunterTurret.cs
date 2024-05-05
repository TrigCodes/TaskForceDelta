/***************************************************************
*file: HunterTurret.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behavior for Hunter Turret
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterTurret : Turret
{
    [Header("Special Upgrade Attributes")]
    [SerializeField] private int specialUpgradeCost = 100;
    [SerializeField] private float enhancementRange = 4f; // Range to find and enhance nearby turrets

    private HashSet<Turret> enhancedTurrets = new HashSet<Turret>();
    private bool specialUpgradeDone = false;

    // function: Update
    // purpose: called every frame to handling turert behavior
    protected override void Update()
    {
        base.Update();
        CheckForStealthEnemies(); // For stealth visibility effect

        // Constantly call to make sure newly placed turrets get effect
        if (specialUpgradeDone)
        {
            EnhanceNearbyTurrets();
        }
    }
    // function: OnDestroy
    // purpose: handling event when gameObject is destroyed
    protected override void OnDestroy()
    {
        if (specialUpgradeDone)
        {
            foreach (Turret turret in enhancedTurrets)
            {
                turret.CanSeeStealthEnemies = false;
            }
        }

        base.OnDestroy();
    }
    // function: CheckForStealthEnemies
    // purpose: check for stealth enemies
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
    // function: TargetAndShoot
    // purpose: get target to shot at
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
                RotateTowards(closestEnemy.position);
                Shoot(closestEnemy.position, true);
                timeSinceLastShot = 0;
            }
            else
            {
                timeSinceLastShot += Time.deltaTime;
            }
        }
    }
    // function: Shoot
    // purpose: shoot at target position
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
            // Sees stealth enemies regardless of canSeeStealthEnemies value
            bulletScript.SetTargetTags(new List<string> { "Enemy", "StealthEnemy" });
        }
    }
    // function: UpgradeSpecial
    // purpose: upgrade unique turret behavior
    public override bool UpgradeSpecial()
    {
        if (!specialUpgradeDone && LevelManager.main.SpendScraps(specialUpgradeCost))
        {
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
        return "Surrounding Turrets Can See Stealth Enemies";
    }

    // funcion: EnhanceNearbyTurrets
    // purpose: Make nearby turrets be able to see stealth turrets
    private void EnhanceNearbyTurrets()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enhancementRange);
        HashSet<Turret> currentTurrets = new HashSet<Turret>();

        foreach (Collider2D hit in hits)
        {
            Turret turret = hit.GetComponent<Turret>();
            if (turret != null && turret != this)
            {
                turret.CanSeeStealthEnemies = true;
                currentTurrets.Add(turret);
            }
        }

        // Revert the ability for turrets that are no longer in range
        foreach (Turret turret in enhancedTurrets)
        {
            if (!currentTurrets.Contains(turret))
            {
                turret.CanSeeStealthEnemies = false;
            }
        }

        enhancedTurrets = currentTurrets;
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
    // function: OnDrawGizmosSelected
    // purpose: draw radius when selected
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        // Use a different color to distinguish the ally enhancement range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enhancementRange);
    }
}