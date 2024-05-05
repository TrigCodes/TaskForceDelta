/***************************************************************
*file: BasicEnemy.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
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
        if (other.gameObject.CompareTag("Turret") ||
            other.gameObject.CompareTag("Core") ||
            other.gameObject.CompareTag("Wall"))
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);

                // Bounce back to immitate attacking again
                StartCoroutine(BounceBack());
            }

            // Play audio
            AudioManager.main.PlayAudio(attackAudio, transform, 1);
        }
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