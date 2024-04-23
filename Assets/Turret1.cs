using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret1 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float shootingDistance = 50f;
    private TurretStat stat;
    [SerializeField] float fireRate = 3f;
    public GameObject bullet;
    GameObject target;
    bool canShoot = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // cost, hitpoint, shield, damage, fireRate
        stat = new TurretStat(30, 100, 30,400,3f);
    }

    void Update()
    {
        if (canShoot)
        {
            canShoot = false;
            //Coroutine for delay between shooting
            StartCoroutine(nameof(AllowToShoot));
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

}
