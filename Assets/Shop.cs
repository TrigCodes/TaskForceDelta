using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    void Start() {
        buildManager = BuildManager.instance;
    }

    public void PurchaseTurret1() {
        buildManager.SetTurretToBuild(buildManager.turret1Prefab);
    }
    public void PurchaseTurret2() {
        buildManager.SetTurretToBuild(buildManager.turret2Prefab);
    }
}
