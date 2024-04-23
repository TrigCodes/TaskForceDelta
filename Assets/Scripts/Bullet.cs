using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    public int Damage { get; set; }
    private Vector3 target; // Position of the target enemy

    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        if (target != null)
        {
            Debug.Log("Target");
            // Move the bullet towards the target
            Vector2 turretToEnemy = target - transform.position;
            turretToEnemy.Normalize();
            rb.velocity = turretToEnemy * speed;
            // transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Bound"))
        {
            Destroy(gameObject);
        }
    }

    // Set the target position for the bullet to move towards
    public void SetTarget(Vector3 targetPosition)
    {
        target = targetPosition;
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
