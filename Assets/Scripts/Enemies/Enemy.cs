using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] protected Rigidbody2D enemyRigidBody; // Allows enemy movement

    [Header("Attributes")]
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected int scrapReward = 5;

    protected Transform target; // Where enemy will target

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GetClosestTarget();
    }

    protected virtual void Update()
    {
        GetClosestTarget();
    }

    protected virtual void OnDestroy()
    {
        if (LevelManager.main != null)
        {
            LevelManager.main.AddScraps(scrapReward);
        }
    }

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


    protected virtual void GetClosestTarget()
    {
        // Get all possible targets in the scene
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        GameObject[] cores = GameObject.FindGameObjectsWithTag("Core");
        List<GameObject> targets = new List<GameObject>(turrets);
        targets.AddRange(cores);

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
}