using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = 500;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        Vector3 newPos = new Vector3(collision.transform.position.x-0.5f, collision.transform.position.y-0.5f, collision.transform.position.z);
        collision.transform.position = newPos;
    }
    public void DamageHealth(int damage) {
        health -= damage;
    }
}
