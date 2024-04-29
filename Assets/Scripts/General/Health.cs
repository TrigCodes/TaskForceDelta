using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // Maximum health
    private int currentHealth; // Current health

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Decrease health by damage amount
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the object when health depletes
        }
    }
}