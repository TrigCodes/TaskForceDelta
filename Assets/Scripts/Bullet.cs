using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    public int Damage {get;set;}
    private Vector3 target; // Position of the target enemy

    void Update()
    {
        // Move the bullet towards the target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Bullet enter enemy");
            Destroy(gameObject);
        }
    }

    // Set the target position for the bullet to move towards
    public void SetTarget(Vector3 targetPosition)
    {
        target = targetPosition;
    }
}
