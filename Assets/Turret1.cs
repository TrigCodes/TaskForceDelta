using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret1 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] float shootingDistance = 50f;
    [SerializeField] float speedbullet = 5f;
    [SerializeField] float fireRate = 3f;
    public GameObject bullet;
    GameObject target;
    bool canShoot = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (canShoot)
        {
            canShoot = false;
            //Coroutine for delay between shooting
            StartCoroutine("AllowToShoot");
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
        //link to spawned bullet, you dont need it, if the bullet has own moving script
        GameObject tpmBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        // tpmBullet.transform.right = direction;
        tpmBullet.GetComponent<Bullet>().SetTarget(target.transform.position);
    }

    IEnumerator AllowToShoot()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

}
