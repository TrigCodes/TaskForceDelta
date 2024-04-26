using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Transform target; // Position of the target enemy
    [Header("Bullet Stat")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float explosionRad = 0;
    public int Damage { get; set; }
    public bool CanSee { get; set; }
    public string TargetType { get; set; }

    void Start()
    {
        Destroy(gameObject, 5f);
    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        if (direction.magnitude <= distanceThisFrame)
        {
            return;
        }
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(TargetType))
        {
            if (explosionRad > 0f)
            {
                Explode();
            }
            else
            {
                DamageEnemy(other.transform);
            }
            Destroy(gameObject);
        }
    }
    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRad);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == TargetType)
            {
                DamageEnemy(collider.transform);
            }
        }
    }
    void DamageEnemy(Transform enemy)
    {
        if (TargetType == "Enemy")
        {
            Enemy e = enemy.GetComponent<Enemy>();
            e.GetDamaged(Damage); ;
        }
        else if (TargetType == "Turret")
        {
            Turret t = enemy.GetComponent<Turret>();
            t.GetDamaged(Damage); ;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRad);
    }
    // Set the target position for the bullet to move towards
    public void SetTarget(Transform targetPosition)
    {
        target = targetPosition;
    }

}
