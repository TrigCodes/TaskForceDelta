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
        if (!buildManager.CanBuild)
        {
            return;
        }
        if (turret != null)
        {
            Debug.Log("Can't built there");
            return;
        }

    buildManager.BuildOn(this);
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (!buildManager.CanBuild)
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
