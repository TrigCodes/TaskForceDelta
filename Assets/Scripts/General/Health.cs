using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // Maximum health
    [SerializeField] private int maxShield = 50; // Maximum shield
    [SerializeField] private float shieldRegenTime = 5f; // Time to fully regenerate shield
    [SerializeField] private GameObject shieldPrefab; // Prefab for shield visualization

    private int currentHealth; // Current health
    private int currentShield; // Current shield
    private GameObject shieldVisual; // Instance of the shield prefab
    private float lastDamageTime; // Time since last damage

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    // For Shield upgrades
    public void UpgradeMaxShield(int incrementPerShieldLevel)
    {
        maxShield += incrementPerShieldLevel;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        if (maxShield > 0)
        {
            // Instantiate shield and make it visable
            currentShield = maxShield;
            shieldVisual = Instantiate(shieldPrefab, transform.position, Quaternion.identity, transform);
            UpdateShieldVisibility();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If no damage was taken for the shieldRegenTime then regenate shield to full
        if (maxShield > 0 && Time.time - lastDamageTime > shieldRegenTime && currentShield < maxShield)
        {
            RegenerateShield();
        }
    }

    public void TakeDamage(int damage)
    {
        lastDamageTime = Time.time;

        // If shield is active
        if (currentShield > 0)
        {
            int shieldDamage = Mathf.Min(currentShield, damage);
            currentShield -= shieldDamage;
            UpdateShieldVisibility();
        }
        // If shield is down
        else
        {
            currentHealth -= damage; // Decrease health by damage amount

            // Update UI if Core
            if (gameObject.tag != null && gameObject.tag == "Core")
            {
                LevelManager.main.HUD.GetComponent<TopHUD>().UpdateCoreHPDisplay(currentHealth);
            }

            if (currentHealth <= 0)
            {
                Destroy(gameObject); // Destroy the object when health depletes
            }
        }
    }

    private void RegenerateShield()
    {
        currentShield = maxShield;
        UpdateShieldVisibility();
    }

    private void UpdateShieldVisibility()
    {
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(currentShield > 0);
            var renderer = shieldVisual.GetComponent<Renderer>();
            if (renderer != null)
            {
                float opacity = 0.8f * (float)currentShield / maxShield;
                Color color = renderer.material.color;
                color.a = opacity;
                renderer.material.color = color;
            }
        }
    }
}