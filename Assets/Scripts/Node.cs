using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [SerializeField] private Color hoverColor;
    private Color defaultColor;
    private SpriteRenderer spriteRenderer;

    private GameObject turret;
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (buildManager.GetTurretToBuild() == null)
        {
            return;
        }
        if (turret != null)
        {
            Debug.Log("Can't built there");
            return;
        }

        GameObject turretToBuild = buildManager.GetTurretToBuild();

        turret = Instantiate(turretToBuild, transform.position, transform.rotation);
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (buildManager.GetTurretToBuild() == null)
        {
            return;
        }
        spriteRenderer.color = hoverColor;
    }
    void OnMouseExit()
    {
        spriteRenderer.color = defaultColor;
    }
}
