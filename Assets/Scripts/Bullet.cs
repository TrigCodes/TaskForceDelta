using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    public int Damage { get; set; }
    private Transform target; // Position of the target enemy

    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        if (target  == null) {
            Destroy(gameObject);
            return;
        }
        Vector2 direction = target.position - transform.position;
        float distanceThisFrame = speed*Time.deltaTime;
        if (direction.magnitude <= distanceThisFrame) {
            // HitTarget();
            return;
        }
        transform.Translate(direction.normalized*distanceThisFrame, Space.World);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    // Set the target position for the bullet to move towards
    public void SetTarget(Transform targetPosition)
    {
        target = targetPosition;
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
