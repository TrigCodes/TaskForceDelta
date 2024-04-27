using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    private Color defaultColor;
    private SpriteRenderer spriteRenderer;

    public GameObject turret;
    public UpgradeTurret upgradeTurret;
    BuildManager buildManager;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        buildManager = BuildManager.instance;
    }
    // TODO: display on screen
    void OnMouseDown()
    {
        Debug.Log("Click on Node");
        // Prevent from clicking over game ui
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("1");
            return;
        }
        // If there are turret on this node, choose that turret
        if (turret != null)
        {
            buildManager.SelectTurret(turret.GetComponent<Turret>());
            Debug.Log("Node selected");
            return;
        }
        // If there is turret blueprint selected
        if (buildManager.CanBuild)
        {
            Debug.Log("2");
            BuildTurret(buildManager.GetTurretToBuild());
            return;
        }
    }
    void BuildTurret(TurretBluePrint bluePrint)
    {
        if (Base.Money < bluePrint.initialCost)
        {
            Debug.Log("Not enough money");
            return;
        }
        Base.Money -= bluePrint.initialCost;
        GameObject turret = Instantiate(bluePrint.prefab, transform.position, Quaternion.identity);
        this.turret = turret;
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!buildManager.CanBuild) { return; }
        spriteRenderer.color = hoverColor;
    }
    void OnMouseExit()
    {
        spriteRenderer.color = defaultColor;
    }
}
