using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float range = 5f;
    [SerializeField] protected int damage = 10;
    [SerializeField] public int cost = 150;

    // Upgrade management
    [Header("Damage Upgrade Attributes")]
    [SerializeField] public int costPerDamageUpgrade = 50;
    [SerializeField] public int damageLevel = 0;
    [SerializeField] protected int incPerDamageLevel = 10;
    [SerializeField] public int damageMaxLevel = 3;

    [Header("Fire Rate Upgrade Attributes")]
    [SerializeField] public int costPerFireRateUpgrade = 30;
    [SerializeField] public int fireRateLevel = 0;
    [SerializeField] protected int incPerFireRateLevel = 10;
    [SerializeField] public int fireRateMaxLevel = 3;

    [Header("Shield Upgrade Attributes")]
    [SerializeField] public int costPerShieldUpgrade = 40;
    [SerializeField] public int shieldLevel = 0;
    [SerializeField] protected int incPerShieldLevel = 10;
    [SerializeField] public int shieldMaxLevel = 3;

    protected float timeSinceLastShot;
    // Static reference for player-controlled turret
    protected static Turret playerControlledTurret;
    // Needed to make sure clicking on turret doesn't trigger shot
    protected BoxCollider2D turretCollider;
    public bool CanSeeStealthEnemies { get; set; } = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {

        turretCollider = GetComponent<BoxCollider2D>();
        timeSinceLastShot = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // Switch modes based on if turret is player controlled or not
        if (this == playerControlledTurret)
        {
            HandlePlayerControl();
        }
        else
        {
            TargetAndShoot();
        }
    }

    protected virtual void OnDestroy()
    {
        BottomHUD uiManager = FindObjectOfType<BottomHUD>();

        if (uiManager != null && playerControlledTurret == this)
        {
            // Disable player control
            playerControlledTurret = null;

            // Hide UI
            if (uiManager != null)
            {
                uiManager.HideHUD();
            }
        }
    }

    protected void OnMouseDown()
    {
        BottomHUD uiManager = FindObjectOfType<BottomHUD>();

        if (playerControlledTurret == this)
        {
            // Disable player control
            playerControlledTurret = null;

            // Hide UI
            if (uiManager != null)
            {
                uiManager.HideHUD();
            }
        }
        else
        {
            // Bring player control to this turret
            playerControlledTurret = this;

            // Display information in UI
            if (uiManager != null)
            {
                uiManager.SetCurrentTurret(this);
            }
        }
    }

    protected virtual void HandlePlayerControl()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Z needs to be on the same plane as the turret

        // Rotate turret to follow the mouse as the mouse is moving around
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Check to see if mouse is clicked
        if (Input.GetMouseButton(0) && timeSinceLastShot >= 1f / fireRate)
        {
            // Check if the mouse click is not on the turret itself
            if (!turretCollider.bounds.Contains(mousePosition))
            {
                Shoot(mousePosition, CanSeeStealthEnemies);
                timeSinceLastShot = 0;
            }
        }
        else
        {
            // For fire rate
            timeSinceLastShot += Time.deltaTime;
        }
    }

    // When player is not controlling
    protected virtual void TargetAndShoot()
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
                if (entity.gameObject.CompareTag("Enemy") || 
                   (CanSeeStealthEnemies && 
                        entity.gameObject.CompareTag("StealthEnemy")
                   )
                )
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
                Shoot(closestEnemy.position, CanSeeStealthEnemies);
                timeSinceLastShot = 0;
            }
            else
            {
                timeSinceLastShot += Time.deltaTime;
            }
        }
    }

    protected abstract void Shoot(Vector3 targetPosition, bool canSeeStealthEnemies);

    // To see turret view radius in the scene editor
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // Methods to upgrade each aspect
    public bool UpgradeDamage()
    {
        if (damageLevel < damageMaxLevel && LevelManager.main.SpendScraps(costPerDamageUpgrade))
        {
            damageLevel++;
            damage += incPerDamageLevel;
            return true;
        }
        else
            return false;
    }

    public bool UpgradeFireRate()
    {
        if (fireRateLevel < fireRateMaxLevel && LevelManager.main.SpendScraps(costPerFireRateUpgrade))
        {
            fireRateLevel++;
            fireRate += incPerFireRateLevel;
            return true;
        }
        else
            return false;
    }

    public bool UpgradeShield()
    {
        if (shieldLevel < shieldMaxLevel && LevelManager.main.SpendScraps(costPerShieldUpgrade))
        {
            shieldLevel++;
            GetComponent<Health>().UpgradeMaxShield(incPerShieldLevel);
            return true;
        }
        else
            return false;
    }

    public abstract bool UpgradeSpecial();
    public abstract bool GetSpecialUpgradeDone();
    public abstract int GetSpecialUpgradeCost();
}