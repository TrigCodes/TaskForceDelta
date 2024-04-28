using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Rigidbody2D enemyRigidBody; // Allows enemy movement

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int damage = 10;

    private Transform target; // Where enemy will target
    private bool isBouncing = false; // To immitate recoil of attacking

    // Start is called before the first frame update
    void Start()
    {
        GetClosestTarget();
    }

    // Update is called once per frame
    void Update()
    {
        GetClosestTarget();
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

    private void MoveTowardsTarget()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            enemyRigidBody.velocity = direction * moveSpeed;
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

    private void GetClosestTarget()
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
