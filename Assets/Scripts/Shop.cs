using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    // [SerializeField] private TurretBluePrint turret1;
    public TurretBluePrint turret1;
    public TurretBluePrint turret2;
    // [SerializeField] private TurretBluePrint turret2;

    void Start() {
        buildManager = BuildManager.instance;
    }

    public void SelectTurret1() {
        buildManager.SetTurretToBuild(turret1);
    }
    public void SelectTurret2() {
        buildManager.SetTurretToBuild(turret2);
    }
}
