using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret1 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float shootingDistance = 50f;
    public TurretStat stat;
    [SerializeField] private GameObject bullet;
    private GameObject target;
    bool canShoot = true;
    private float dmgCooldown;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // cost, hitpoint, shield, damage, fireRate
        stat = new TurretStat(30, 100, 30, 100, 2f);
        dmgCooldown = 0.0f;
    }

    void Update()
    {
        if (canShoot)
        {
            canShoot = false;
            //Coroutine for delay between shooting
            StartCoroutine(AllowToShoot());
            //array with enemies
            //you can put in start, iff all enemies are in the level at beginn (will be not spawn later)
            GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Enemy");
            if (allTargets != null)
            {
                target = allTargets[0];
                //look for the closest
                foreach (GameObject tmpTarget in allTargets)
                {
                    if (Vector2.Distance(transform.position, tmpTarget.transform.position) < Vector2.Distance(transform.position, target.transform.position))
                    {
                        target = tmpTarget;
                    }
                }
                //shoot if the closest is in the fire range
                if (Vector2.Distance(transform.position, target.transform.position) < shootingDistance)
                {
                    Fire();
                }
            }
        }

    }
    // void FixedUpdate()
    // {
    //     // Check for death
    //     if (stat.Hitpoint <= 0)
    //     {
    //         Destroy(gameObject);
    //     }
    // }
    void Fire()
    {
        GameObject tpmBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        tpmBullet.GetComponent<Bullet>().Damage = stat.Damage;
        tpmBullet.GetComponent<Bullet>().SetTarget(target.transform.position);
    }

    IEnumerator AllowToShoot()
    {
        yield return new WaitForSeconds(stat.FireRate);
        canShoot = true;
    }
    // void OnCollisionEnter2D(Collision2D other)
    // {

    // }
    // void OnCollisionStay2D(Collision2D other)
    // {
    //     // dmgCooldown += Time.deltaTime;
    //     // if (dmgCooldown > 1) {
    //     stat.Hitpoint -= other.gameObject.GetComponent<Enemies1>().stat.Damage;
    //     Debug.Log(stat.Hitpoint);
    //     StartCoroutine(TakeDamageCooldown());
    //     // dmgCooldown = 0.0f;
    //     // }

    //     // while (dmgCooldown <= 2) {
    //     //     dmgCooldown += Time.deltaTime;
    //     // }
    //     // if (other.CompareTag("Enemy")) {
    //     //     float dps = other.gameObject.GetComponent<Turret1>().stat.Damage*Time.deltaTime;
    //     //     stat.Hitpoint -= (int)dps;
    //     // }
    // }
    // IEnumerator TakeDamageCooldown()
    // {
    //     Debug.Log("Waited");
    //     yield return new WaitForSeconds(2);
    // }

}
