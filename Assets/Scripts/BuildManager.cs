using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one BuildManager in scene");
            return;
        }
        instance = this;
    }

    public GameObject turret1Prefab;
    public GameObject turret2Prefab;
    private TurretBluePrint turretToBuild;

    public bool CanBuild {get{return turretToBuild != null;}}

    public void BuildOn(Node node) {
        if (Base.Money < turretToBuild.cost) {
            Debug.Log("Not enough money");
            return ;
        }
        Base.Money -= turretToBuild.cost;
        GameObject turret = Instantiate(turretToBuild.prefab, node.transform.position, Quaternion.identity);
        node.turret = turret;
    }
    public void SetTurretToBuild(TurretBluePrint turret) {
        turretToBuild = turret;
    }
}
