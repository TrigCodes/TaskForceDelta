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
    [SerializeField] protected int damage = 20;
    [SerializeField] protected int maxHealth = 100;
    protected float timeSinceLastShot;

    // Static reference for player-controlled turret
    protected static Turret playerControlledTurret;

    // Needed to make sure clicking on turret doesn't trigger shot
    protected BoxCollider2D turretCollider;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Set max health
        this.GetComponent<Health>().SetMaxHealth(maxHealth);

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

    protected void OnMouseDown()
    {
        if (playerControlledTurret == this)
        {
            // Disable player control
            playerControlledTurret = null;
        }
        else
        {
            // Bring player control to this turret
            playerControlledTurret = this;
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
                Shoot(mousePosition);
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
                if (entity.gameObject.CompareTag("Enemy"))
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

    protected abstract void Shoot(Vector3 targetPosition);

    // To see turret view radius in the scene editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}