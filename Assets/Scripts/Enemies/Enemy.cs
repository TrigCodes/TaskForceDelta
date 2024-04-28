using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] protected Rigidbody2D enemyRigidBody; // Allows enemy movement

    protected float moveSpeed;
    protected int damage;

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

    protected virtual void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            enemyRigidBody.velocity = direction * moveSpeed;
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