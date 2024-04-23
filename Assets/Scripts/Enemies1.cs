using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies1 : MonoBehaviour
{
    [SerializeField] private Transform playerBase;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float knockback;
    [SerializeField] public EnemyStat stat;
    void Start() {
        GameObject b = GameObject.FindWithTag("Base");
        if (b != null) {
            playerBase = b.GetComponent<Transform>();
        } else {
            Debug.LogError("Cannot find Base");
        }
        rb = gameObject.GetComponent<Rigidbody>();
        stat = new EnemyStat(1.0f, 30, 500);
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position = Vector2.MoveTowards(transform.position, playerBase.position, moveSpeed*Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet")) {
            GetDamaged(other.gameObject.GetComponent<Bullet>().Damage);
        }
    }
    void GetDamaged(int damage) {
        stat.Hitpoint -= damage;
        if (stat.Hitpoint <= 0) {
            Destroy(gameObject);
        }
    }
}
