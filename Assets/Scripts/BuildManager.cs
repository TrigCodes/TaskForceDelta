using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    [SerializeField] private GameObject upgradeUI;

    void Awake() {
        if (instance != null) {
            Debug.LogError("More than one BuildManager in scene");
            return;
        }
        instance = this;
        upgradeUI.SetActive(false);
    }

    private TurretBluePrint turretToBuild;
    private Turret selectedTurret;
    public bool CanBuild {get{return turretToBuild != null;}}
    public bool HaveTurret {get{return selectedTurret != null;}}


    public void SetTurretToBuild(TurretBluePrint turret) {
        turretToBuild = turret;
        selectedTurret = null;
        upgradeUI.SetActive(false);
        Debug.Log("Selected Turret To Build");
    }
    public void SelectTurret(Turret turret) {
        selectedTurret = turret;
        turretToBuild = null;
        upgradeUI.SetActive(true);
        Debug.Log("Turret Selected");
    }
    public TurretBluePrint GetTurretToBuild() {
        return turretToBuild;
    }
    public Turret GetTurret() {
        return selectedTurret;
    }
    }
