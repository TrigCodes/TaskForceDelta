/***************************************************************
*file: Turret.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide general behavior for turret
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firingPoint;
    [SerializeField] protected AudioClip shootAudio;
    [SerializeField] public AudioClip deathAudio;

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
    [SerializeField] protected float incPerFireRateLevel = 10f;
    [SerializeField] public int fireRateMaxLevel = 3;

    [Header("Shield Upgrade Attributes")]
    [SerializeField] public int costPerShieldUpgrade = 40;
    [SerializeField] public int shieldLevel = 0;
    [SerializeField] protected int incPerShieldLevel = 10;
    [SerializeField] public int shieldMaxLevel = 3;

    [Header("Annimaion")]
    [SerializeField] private Sprite[] sprites; // Array to hold the sprites
    [SerializeField] private float changeInterval = 1.0f; // Interval in seconds between sprite changes

    protected float timeSinceLastShot;
    // Static reference for player-controlled turret
    protected static Turret playerControlledTurret;
    // Needed to make sure clicking on turret doesn't trigger shot
    protected BoxCollider2D turretCollider;
    public bool CanSeeStealthEnemies { get; set; } = false;
    protected SpriteRenderer spriteRenderer; // SpriteRenderer component on the GameObject
    protected Coroutine spriteAnimationCoroutine;

    // function: Start
    // purpose: Start is called before the first frame update to get gamObject info
    protected virtual void Start()
    {
        // For animation
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        Rigidbody2D turretRigidBody = GetComponent<Rigidbody2D>();
        if (turretRigidBody != null)
        {
            turretRigidBody.freezeRotation = true; // Freeze rotation in physics calculations
        }

        turretCollider = GetComponent<BoxCollider2D>();
        timeSinceLastShot = 0;
    }
    // function: Update
    // purpose: Update is called once per frame to change behavior of turret
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

        if (CanSeeStealthEnemies)
        {
            CheckForStealthEnemies();
        }
    }
    // function: OnDestroy
    // purpose: handling behavior when gamObject is destroyed
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
    // function: OnMouseDown
    // purpose: handling event when player mouse click on turret to control or not
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
    // function: HandlePlayerControl
    // purpose: change turret behavior if player take control
    protected virtual void HandlePlayerControl()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Z needs to be on the same plane as the turret

        // Rotate turret to follow the mouse as the mouse is moving around
        RotateTowards(mousePosition);

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
    // function: TargetAndShoot
    // purpose: When player is not controlling, this is the default behavior
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
                RotateTowards(closestEnemy.position);
                Shoot(closestEnemy.position, CanSeeStealthEnemies);
                timeSinceLastShot = 0;
            }
            else
            {
                timeSinceLastShot += Time.deltaTime;
            }
        }
    }
    // function: RotateTowards
    // purpose: rotate turret toward target position
    protected virtual void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 270f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    // function: Shoot
    // purpose: handling shooting at the target
    protected virtual void Shoot(Vector3 targetPosition, bool canSeeStealthEnemies)
    {
        // Play audio
        AudioManager.main.PlayAudio(shootAudio, transform, 1);

        // If the animation is already running, stop it
        if (spriteAnimationCoroutine != null)
            StopCoroutine(spriteAnimationCoroutine);

        // Restart the animation coroutine
        spriteAnimationCoroutine = StartCoroutine(CycleSprites());
    }
    // function: OnDrawGizmosSelected
    // purpose: To see turret view radius in the scene editor
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // function: UpgradeDamage
    // purpose: upgrade turret damage
    public bool UpgradeDamage()
    {
        if (damageLevel < damageMaxLevel && LevelManager.main.SpendScraps(costPerDamageUpgrade))
        {
            damageLevel++;
            damage += incPerDamageLevel;
            return true;
        }
        else
        {
            FindObjectOfType<Alert>().DisplayAlert($"Not Enough Scraps for Upgrade");
            return false;
        }
    }
    // function: UpgradeFireRate
    // purpose: upgrade turret fire rate
    public bool UpgradeFireRate()
    {
        if (fireRateLevel < fireRateMaxLevel && LevelManager.main.SpendScraps(costPerFireRateUpgrade))
        {
            fireRateLevel += 1;
            fireRate += incPerFireRateLevel;

            costPerFireRateUpgrade += 50;

            return true;
        }
        else
        {
            FindObjectOfType<Alert>().DisplayAlert($"Not Enough Scraps for Upgrade");
            return false;
        }
    }
    // function: UpgradeShield
    // purpose: upgrade turret shield
    public bool UpgradeShield()
    {
        if (shieldLevel < shieldMaxLevel && LevelManager.main.SpendScraps(costPerShieldUpgrade))
        {
            shieldLevel++;
            GetComponent<Health>().UpgradeMaxShield(incPerShieldLevel);

            costPerShieldUpgrade += 50;

            return true;
        }
        else
        {
            FindObjectOfType<Alert>().DisplayAlert($"Not Enough Scraps for Upgrade");
            return false;
        }
    }
    // function: CheckForStealthEnemies
    // purpose: handling if turret can see stealth enemies
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

    // abstract function to be implemented for special upgrade
    public abstract bool UpgradeSpecial();
    public abstract string GetSpecialInfoText();
    public abstract bool GetSpecialUpgradeDone();
    public abstract int GetSpecialUpgradeCost();

    // function: CycleSprites
    // purpose: cycling sprites
    protected IEnumerator CycleSprites()
    {
        int index = 0;
        bool goingForward = true;

        while (true) // Loop until the full cycle is complete
        {
            spriteRenderer.sprite = sprites[index];

            if (goingForward)
            {
                if (index < sprites.Length - 1)
                    index++;
                else
                    goingForward = false;
            }
            else
            {
                if (index > 0)
                    index--;
                else
                    break; // Exit the loop once the cycle is complete
            }

            yield return new WaitForSeconds(changeInterval);
        }

        spriteAnimationCoroutine = null; // Reset the coroutine reference when done
    }
}