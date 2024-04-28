using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private bool isBouncing = false; // To immitate recoil of attacking

    protected override void Start()
    {
        // Custom variables for Machine Gun Turret
        // Overrides values when game starts
        moveSpeed = 5;
        damage = 10;

        base.Start();  // Call base to ensure any base initialization happens
    }

    protected override void Update()
    {
        base.Update();
        if (!isBouncing)
        {
            MoveTowardsTarget();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Turret") || other.gameObject.CompareTag("Core"))
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);

                // Bounce back to immitate attacking again
                StartCoroutine(BounceBack());
            }
        }
    }

    // Need to bounce back if squished by other enemies
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Turret") || other.gameObject.CompareTag("Core"))
        {
            StartCoroutine(BounceBack());
        }
    }

    private IEnumerator BounceBack()
    {
        isBouncing = true;
        Vector2 direction = (transform.position - target.position).normalized;
        enemyRigidBody.velocity = direction * moveSpeed;
        yield return new WaitForSeconds(0.5f);
        isBouncing = false;
    }
}
