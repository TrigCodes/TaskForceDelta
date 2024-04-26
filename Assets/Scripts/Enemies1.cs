using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies1 : MonoBehaviour
{

    [SerializeField] private Transform playerBase;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float knockback;

    [Header("Stat")]
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private int damage = 30;
    [SerializeField] private int hitpoint = 500;

    void Start() {
        GameObject b = GameObject.FindWithTag("Base");
        if (b != null) {
            playerBase = b.GetComponent<Transform>();
        } else {
            Debug.Log("Cannot find Base");
        }
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        transform.position = Vector2.MoveTowards(transform.position, playerBase.position, moveSpeed*Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet")) {
            GetDamaged(other.gameObject.GetComponent<Bullet>().Damage);
        } else if (other.gameObject.CompareTag("Base")) {
            Base.BaseHealth -= damage;
            Debug.Log("Damage base for: " + damage);
        }
    }
    void GetDamaged(int damage) {
        hitpoint -= damage;
        if (hitpoint <= 0) {
            Destroy(gameObject);
        }
    }
}
