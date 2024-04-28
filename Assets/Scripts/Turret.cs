using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    BuildManager buildManager;
    [SerializeField] private GameObject bullet;
    private Transform target;
    private float fireCountdown = 0.0f;

    [Header("Turret Stat")]
    [SerializeField] private int hitPoint = 30;
    [SerializeField] private int shield = 30;
    [SerializeField] private float range = 10f;
    [SerializeField] private int damage = 100;
    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private bool canSee = false;

    [Header("Upgrade Stat")]
    [SerializeField] private int upDamage = 5;
    [SerializeField] private float upFireRate = 0.5f;
    [SerializeField] private int upShield = 10;
    [SerializeField] private int[] upDamageCost = { 30, 50, 80 };
    [SerializeField] private int[] upFireRateCost = { 50, 80, 100 };
    [SerializeField] private int[] upShieldCost = { 30, 50, 80 };
    private const int MAX_LEVEL = 3;
    private int lvDamage = 0;
    private int lvFireRate = 0;
    private int lvShield = 0;
    private bool takeControl = false;
    [SerializeField] private bool shieldRegenAllow = true;
    [SerializeField] private int maxShield;


    void Start()
    {
        buildManager = BuildManager.instance;
        maxShield = shield;
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
        InvokeRepeating(nameof(ShieldRegeneration), 1, 2);
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            // Can choose target if turret can see, or target can be seen
            bool canChoose = canSee ? canSee : enemy.GetComponent<Enemy>().BeSeen;

            if (distanceToEnemy < shortestDistance && canChoose)
            {
                shortestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }
        if (closestEnemy != null && shortestDistance <= range)
        {
            target = closestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
    void Update()
    {
        if (target == null && !takeControl)
        {
            return;
        }
        if (fireCountdown <= 0)
        {
            Fire();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    // TODO: bullet spawn location needed to change.
    void Fire()
    {
        GameObject bulletSpawn = Instantiate(bullet, transform.position, Quaternion.identity);
        Bullet bulletProp = bulletSpawn.GetComponent<Bullet>();

        if (bullet != null)
        {
            bulletProp.Damage = damage;
            bulletProp.CanSee = canSee;
            bulletProp.TargetType = "Enemy";
            bulletProp.SetTarget(target);
            if (takeControl) bulletProp.ManualControl = takeControl;

        }
    }
    public void GetDamaged(int damage)
    {
        if (shield >= damage)
        {
            shield -= damage;
            return;
        }
        else
        {
            int excess = damage - shield;
            hitPoint -= excess;
            shield = 0;
        }
        StartCoroutine(DisableShieldRegen());

        if (hitPoint <= 0)
        {
            CancelInvoke(nameof(ShieldRegeneration));
            CancelInvoke(nameof(UpdateTarget));
            Destroy(gameObject);
        }
    }
    void ShieldRegeneration()
    {
        if (shieldRegenAllow && (shield < maxShield))
        {
            shield++;
        }
    }
    IEnumerator DisableShieldRegen()
    {
        shieldRegenAllow = false;
        yield return new WaitForSeconds(5);
        shieldRegenAllow = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    public void UpgradeDamage()
    {
        if (lvDamage < MAX_LEVEL)
        {
            if (Base.Money < upDamageCost[lvDamage])
            {
                Debug.Log("Not enough money");
                return;
            }
            Base.Money -= upDamageCost[lvDamage];
            damage += upDamage;
            lvDamage++;
        }
    }
    public void UpgradeFireRate()
    {
        if (lvFireRate < MAX_LEVEL)
        {
            if (Base.Money < upFireRateCost[lvFireRate])
            {
                Debug.Log("Not enough money");
                return;
            }
            Base.Money -= upFireRateCost[lvFireRate];
            fireRate += upFireRate;
            lvFireRate++;
        }
    }
    public void UpgradeShield()
    {
        if (lvShield < MAX_LEVEL)
        {
            if (Base.Money < upShieldCost[lvShield])
            {
                Debug.Log("Not enough money");
                return;
            }
            Base.Money -= upShieldCost[lvShield];
            shield += upShield;
            maxShield = shield;
            lvShield++;
        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            buildManager.SelectTurret(this);
            takeControl = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            takeControl = true;
        }
        Debug.Log("Take control: " + takeControl);
    }
}
