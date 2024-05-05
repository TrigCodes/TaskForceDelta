/***************************************************************
*file: ExplosiveBullet.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behaavior for explosive bullet
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    [Header("Explosive Bullet Attributes")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxLifetime = 2f;

    private List<string> targetTags; // List of tags that this bullet can hit
    private List<string> collateralTags; // List of tage that take collateral damage
    private Vector2 direction;
    private bool isFired = false;
    private int damage;
    private float blastRadius;
    private SpriteRenderer bulletRenderer; // For hiding bullet on impact
    private bool hasHitTarget = false;

    // function: Start
    // purpose: Called before the first frame update to get gameObject necessary info.
    void Start()
    {
        bulletRenderer = GetComponent<SpriteRenderer>();

        // To let animation run without bullet being destroyed
        Invoke("DestroyIfNotHit", maxLifetime);
    }

    // function: DestroyIfNotHit
    // purpose: Destroy object if it doesn't hit anything
    private void DestroyIfNotHit()
    {
        if (!hasHitTarget)
        {
            Destroy(gameObject);
        }
    }

    // function: Update
    // purpose: Update is called once per frame to move toward a direction
    void Update()
    {
        // Wait for direction to be set by shooter
        if (isFired)
        {
            // Move the bullet in the set direction
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }
    // function: SetDamage
    // purpose: Set bullet damage
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
    // function: SetBlastRadius
    // purpose: set bullet radius of explosion
    public void SetBlastRadius(float radius)
    {
        blastRadius = radius;
    }
    // function: SetTargetTags
    // purpose: set list of target bullet can hit
    public void SetTargetTags(List<string> tags)
    {
        targetTags = new List<string>(tags);
    }
    // function: SetCollateralTags
    // purpose: set list of target that take collateral damage
    public void SetCollateralTags(List<string> tags)
    {
        collateralTags = new List<string>(tags);
    }
    // function: SetDirection
    // purpose: set bullet direction
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection; // Set bullet direction
        isFired = true; // The bullet is now moving

        // Calculate the angle in radians from the direction vector and add 90 degrees
        // since bullet capsul is facing vertically
        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg + 90;
        // Rotate the bullet to align with the direction
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    // function: OnTriggerEnter2D
    // purpose: determine behavior when it collide with something.
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (targetTags.Contains(hitInfo.gameObject.tag))
        {
            hasHitTarget = true;

            // Stop bullet form moving further
            bulletRenderer.enabled = false; // Hide bullet renderer
            var colliderComponent = GetComponent<Collider2D>(); // Assuming the bullet uses a Collider2D
            if (colliderComponent != null)
            {
                colliderComponent.enabled = false;
            }

            StartCoroutine(ExplodeAndDestroy()); // Start the explosion and destruction sequence
        }
    }
    // function: ExplodeAndDestroy
    // purpose: start the explosion and destruction sequence
    private IEnumerator ExplodeAndDestroy()
    {
        // Spawn the explosion at full size
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.localScale = new Vector3(blastRadius, blastRadius, 1);

        // Apply damage immediately
        DealDamage();

        // Animate explosion shrinking and then destroy
        yield return AnimateExplosion(explosion);

        Destroy(gameObject); // Destroy the bullet object after the explosion finishes
    }
    // function: AnimateExplosion
    // purpose: Animation for the explosion
    private IEnumerator AnimateExplosion(GameObject explosion)
    {
        float duration = 0.5f; // Duration of the fade out and shrink animation
        float timer = duration;
        SpriteRenderer explosionRenderer = explosion.GetComponent<SpriteRenderer>();

        while (timer > 0)
        {
            float scale = Mathf.Lerp(0, blastRadius*2, timer / duration);
            float alpha = Mathf.Lerp(0, 1, timer / duration);
            explosionRenderer.color = new Color(
                explosionRenderer.color.r,
                explosionRenderer.color.g,
                explosionRenderer.color.b,
                alpha);
            explosion.transform.localScale = new Vector3(scale, scale, 1);
            timer -= Time.deltaTime;
            yield return null;
        }

        Destroy(explosion); // Destroy the explosion object after animation
    }
    // purpose: DealDamage
    // function: Deal damage to all applicable target
    private void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, blastRadius);
        foreach (Collider2D enemy in enemies)
        {
            if (collateralTags.Contains(enemy.gameObject.tag))
            {
                Health enemyHealth = enemy.GetComponent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
        }
    }
    // function: OnDrawGizmos
    // purpose: Draw a blast radius
    void OnDrawGizmos()
    {
        if (explosionPrefab != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, blastRadius);
        }
    }
}