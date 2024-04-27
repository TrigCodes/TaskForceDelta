using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTurret : MonoBehaviour
{
    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void UpgradeDamage()
    {
        if (buildManager.HaveTurret)
        buildManager.GetTurret().UpgradeDamage();
        Debug.Log("Damage up");
    }
    public void UpgradeFireRate()
    {
        if (buildManager.HaveTurret)
        buildManager.GetTurret().UpgradeFireRate();
        Debug.Log("Fire rate up");
    }
    public void UpgradeShield()
    {
        if (buildManager.HaveTurret)
        buildManager.GetTurret().UpgradeShield();
        Debug.Log("Shield up");
    }
}
