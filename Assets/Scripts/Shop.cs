using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shop : MonoBehaviour
{
    BuildManager buildManager;

    public TurretBluePrint turret1;
    public TurretBluePrint turret2;
    public TurretBluePrint turret3;

    void Start() {
        buildManager = BuildManager.instance;
    }

    public void SelectTurret1() {
        buildManager.SetTurretToBuild(turret1);
    }
    public void SelectTurret2() {
        buildManager.SetTurretToBuild(turret2);
    }
    public void SelectTurret3() {
        buildManager.SetTurretToBuild(turret3);
    }
}
