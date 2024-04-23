using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    // [SerializeField] private int damage = 5;
    public int Damage {get;set;}
    private Vector3 target; // Position of the target enemy

    void Start() {
        Damage = 300;
    }
    void Update()
    {
        // Move the bullet towards the target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    // Set the target position for the bullet to move towards
    public void SetTarget(Vector3 targetPosition)
    {
        target = targetPosition;
    }
}
