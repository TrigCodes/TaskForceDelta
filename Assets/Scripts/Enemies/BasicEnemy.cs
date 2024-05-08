/***************************************************************
*file: BasicEnemy.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/7/2024
*
*purpose: This class provide behavior for basic enemy.
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private bool isBouncing = false; // To immitate recoil of attacking
    private float attackCooldown = 0.75f; // Cooldown time in seconds between attacks
    private float lastAttackTime = 0; // Time of the last attack
    // function: Update
    // purpose: called every every frame to move toward target if not get bounced
    protected override void Update()
    {
        base.Update();
        if (!isBouncing)
        {
            MoveTowardsTarget();
        }
    }
    // function: OnCollisionEnter2D
    // purpose: determine behavior when game object collide with another
    void OnCollisionEnter2D(Collision2D other)
    {
        HandleCollision(other);
    }
    // function: OnCollisionStay2D
    // purpose: bounce back when collide with another gameObject
    // Need to bounce back if squished by other enemies
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Turret") ||
            other.gameObject.CompareTag("Core") ||
            other.gameObject.CompareTag("Wall"))
        {
            StartCoroutine(BounceBack());
        }
    }
    // function: HandleCollision
    // purpose: Handles collision logic to avoid code duplication
    private void HandleCollision(Collision2D other)
    {
        if (other.gameObject.CompareTag("Turret") ||
            other.gameObject.CompareTag("Core") ||
            other.gameObject.CompareTag("Wall"))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                // Play audio
                AudioManager.main.PlayAudio(attackAudio, transform, 1);

                Health targetHealth = other.gameObject.GetComponent<Health>();
                if (targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
                    lastAttackTime = Time.time; // Update last attack time
                }

                // Bounce back to imitate attacking again
                StartCoroutine(BounceBack());
            }
        }
    }
    // funcion: BounceBack
    // purpose: actual bounce back with time waiting in between
    private IEnumerator BounceBack()
    {
        isBouncing = true;
        Vector2 direction = (transform.position - target.position).normalized;
        enemyRigidBody.velocity = direction * moveSpeed;
        yield return new WaitForSeconds(0.5f);
        isBouncing = false;
    }
}