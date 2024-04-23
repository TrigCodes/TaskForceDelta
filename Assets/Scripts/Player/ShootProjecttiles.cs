using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjecttiles : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the bullet to shoot
    public Transform firePoint; // Point from where the bullets will be fired
    public float shootInterval = 2f; // Time interval between each shot
    public float detectionRange = 10f; // Range within which enemies will be detected

    private float shootTimer; // Timer to track the time between shots

    void Start()
    {
        shootTimer = 0f;
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            ShootAtEnemy();
            shootTimer = 0f;
        }
    }

    void ShootAtEnemy()
    {
        // Find all enemies within detection range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Instantiate bullet prefab and shoot at the enemy
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<Bullet>().SetTarget(collider.transform.position);
                break; // Shoot at the first enemy found
            }
        }
    }
}
