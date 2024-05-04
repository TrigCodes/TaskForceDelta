using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Attributes")]
    [SerializeField] private int maxHealth = 100; // Maximum health
    [SerializeField] private int maxShield = 50; // Maximum shield
    [SerializeField] private float shieldRegenTime = 5f; // Time to fully regenerate shield
    [SerializeField] private GameObject shieldPrefab; // Prefab for shield visualization

    private int currentHealth; // Current health
    private int currentShield; // Current shield
    private GameObject shieldVisual; // Instance of the shield prefab
    private float lastDamageTime; // Time since last damage
    private Coroutine wallRegenCoroutine; // Coroutine for wall regeneration

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

            // If Wall
            if (gameObject.tag != null && gameObject.tag == "Wall")
            {
                UpdateWallOpacity();
                if (currentHealth <= 0)
                {
                    gameObject.tag = "DamagedWall";  // To make enemies not taget the wall
                    gameObject.GetComponent<Collider2D>().enabled = false; // Enemies avoid wall
                    if (wallRegenCoroutine != null)
                        StopCoroutine(wallRegenCoroutine);
                    float wallRegenTimer = GetComponent<Wall>().wallRegenTimer;
                    wallRegenCoroutine = StartCoroutine(WallRegeneration(wallRegenTimer));
                }
            }
            // Update UI if Core
            else if (gameObject.tag != null && gameObject.tag == "Core")
            {
                LevelManager.main.UI.GetComponent<TopHUD>().UpdateCoreHPDisplay(currentHealth);
            }
            // Update UI if Turret
            else if (gameObject.tag != null && gameObject.tag == "Turret")
            {
                LevelManager.main.UI.GetComponent<BottomHUD>().UpdateTurretHP(GetComponent<Turret>(), currentHealth);
            }

            if (currentHealth <= 0)
            {
                if (gameObject.tag != "DamagedWall")
                {
                    Destroy(gameObject); // Destroy the object when health depletes
                }
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

    private void UpdateWallOpacity()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            float opacity = Mathf.Max(0.3f, (float)currentHealth / maxHealth);
            Color color = renderer.material.color;
            color.a = opacity;
            renderer.material.color = color;
        }
    }

    private IEnumerator WallRegeneration(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentHealth = maxHealth;
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.tag = "Wall";
        UpdateWallOpacity();
    }
}