/***************************************************************
*file: Bullet.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide general behavior for bullet
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Attributes")]
    [SerializeField] private float speed = 20f; // Speed at which the bullet travels
    [SerializeField] private float maxLifetime = 2f; // Maximum time before the bullet destroys itself

    private List<string> targetTags; // List of tags that this bullet can hit
    private Vector2 direction; // Direction the bullet will move
    private bool isFired = false; // To ensure the direction is only set once
    private int damage; // Damage the bullet will deal to the enemy

    // function: Start
    // purpose: Called before the first frame update to get gameObject necessary info.
    void Start()
    {
        Destroy(gameObject, maxLifetime); // Automatically destroy the bullet after a set time
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
    // purpose: set bullet damage
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
    // function: SetTargetTags
    // purpose: set list of tag that enemies can hit
    public void SetTargetTags(List<string> tags)
    {
        targetTags = new List<string>(tags);
    }
    // function: SetDirection
    // purpose: set direction of bullet
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
    public void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if the object hit is a target
        if (targetTags.Contains(hitInfo.gameObject.tag))
        {
            // Try to get the Health component on the target
            Health targetHealth = hitInfo.GetComponent<Health>(); // Changed according to user's request
            if (targetHealth != null)
            {
                // If the enemy has a Health component, deal damage
                targetHealth.TakeDamage(damage);
            }
            // Destroy the bullet after hitting the enemy
            Destroy(gameObject);
        }
    }
}