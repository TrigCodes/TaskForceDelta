/***************************************************************
*file: Enemy.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide general behavior for all enemy.
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] protected Rigidbody2D enemyRigidBody; // Allows enemy movement
    [SerializeField] protected AudioClip attackAudio;
    [SerializeField] public AudioClip deathAudio;

    [Header("Attributes")]
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int scrapReward = 5;

    [Header("Annimaion")]
    [SerializeField] private Sprite[] sprites; // Array to hold the sprites
    [SerializeField] private float changeInterval = 1.0f; // Interval in seconds between sprite changes

    protected Transform target; // Where enemy will target
    protected SpriteRenderer spriteRenderer; // SpriteRenderer component on the GameObject

    // function: Start
    // purpose: Called before the first frame update to get gameObject necessary info.
    protected virtual void Start()
    {
        // For animation
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        if (sprites.Length > 0)
        {
            StartCoroutine(CycleSprites()); // Start the coroutine if there are sprites
        }
        if (enemyRigidBody != null)
        {
            enemyRigidBody.freezeRotation = true; // Freeze rotation in physics calculations
        }

        GetClosestTarget();
    }
    // function: Update
    // purpose: Call GetClosestTarget every frame
    protected virtual void Update()
    {
        GetClosestTarget();
    }
    // function: OnDestroy
    // purpose: determine behavior when gameObject is destroyed
    protected virtual void OnDestroy()
    {
        if (LevelManager.main != null)
        {
            LevelManager.main.AddScraps(scrapReward);
        }
    }
    // function: MoveTowardsTarget
    // purpose: determine movement behavior toward target
    protected virtual void MoveTowardsTarget(bool shouldMove = true)
    {
        if (target != null && shouldMove)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            enemyRigidBody.velocity = direction * moveSpeed;
        }
        else
        {
            enemyRigidBody.velocity = Vector2.zero; // Stop moving
        }
    }

    // function: GetClosestTarget
    // purpose: Get the closest target from gameObject
    protected virtual void GetClosestTarget()
    {
        // Get all possible targets in the scene
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        GameObject[] cores = GameObject.FindGameObjectsWithTag("Core");
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        List<GameObject> targets = new List<GameObject>(turrets);
        targets.AddRange(cores);
        targets.AddRange(walls);

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in targets)
        {
            // Check if the potential target is not null (i.e., it hasn't been destroyed)
            if (potentialTarget != null)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }
        }

        target = bestTarget;
    }
    // function: CycleSprites
    // purpose: cycle between sprites
    protected virtual IEnumerator CycleSprites()
    {
        int index = 0; // Start with the first sprite
        bool goingBack = false;

        while (true) // Loop indefinitely
        {
            spriteRenderer.sprite = sprites[index];
            if (goingBack)
            {
                index--;
                if (index <= 0)
                {
                    goingBack = false;
                }
            }
            else
            {
                index++;
                if (index >= sprites.Length - 1)
                {
                    goingBack = true;
                }
            }
            yield return new WaitForSeconds(changeInterval);
        }
    }
}