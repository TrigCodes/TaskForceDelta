using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int totalCoin = 100;
    [SerializeField] private GameObject turret1;
    [SerializeField] TurretStat turretStat1;

    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log(totalCoin);
        //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     Vector3 offset = new Vector3(0, 0, 30);
        //     Instantiate(turret1, mousePos + offset, Quaternion.identity);
        // }
    }
}
