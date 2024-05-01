using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaceableTile : MonoBehaviour
{
    private GameObject currentTurret = null;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Color selectedColor = Color.green;
    public static PlaceableTile selectedTile;
    private bool playerAiming;

    // Start is called before the first frame update
    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {

        // Check if player has control
        BottomHUD uiManager = FindObjectOfType<BottomHUD>();

        if (uiManager.bottomHUD.style.display == DisplayStyle.None)
        {
            playerAiming = false;
        }
        else
        {
            playerAiming = true;

            // Keep HUD hidden if player is aiming
            RightHUD rightHUD = FindObjectOfType<RightHUD>();
            ResetColor();
            rightHUD.HideHUD();
        }

        // Check if tile is free
        if (currentTurret == null)
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    void OnMouseDown()
    {
        if (!playerAiming)
        {
            if (selectedTile != null)
                selectedTile.ResetColor();

            selectedTile = this;
            spriteRenderer.color = selectedColor; // Change color to selectedColor
            ShowTurretOptions(); // Show turret placement options
        }
    }

    void OnMouseEnter()
    {
        // If player is aiming, dont distract them by highlighting placeable tiles
        // Dont show highlight over tiles with turrets already placed
        if (!playerAiming && currentTurret == null && selectedTile != this)
        {
            spriteRenderer.color = Color.magenta;
        }
    }

    void OnMouseExit()
    {
        if (!playerAiming && currentTurret == null)
        {
            if (selectedTile != this)
            {
                ResetColor();
            }
            // If this is the selected tile, do not reset color
        }
    }

    private void ShowTurretOptions()
    {
        RightHUD uiManager = FindObjectOfType<RightHUD>();
        uiManager.ShowHUD();
    }

    public void PlaceTurret(GameObject turretPrefab, int cost)
    {
        if (turretPrefab != null && LevelManager.main.SpendScraps(cost))
        {
            currentTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity, transform);
            GetComponent<Collider2D>().enabled = false;
            ResetColor();
        }
        else
        {
            // Send message that you dont have enough
        }
    }

    public void PlaceWall()
    {
        GameObject wallPrefab = LevelManager.main.wallPrefab;
        if (wallPrefab != null && LevelManager.main.SpendWall())
        {
            Instantiate(wallPrefab, transform.position, Quaternion.identity);
            ResetColor();
            Destroy(gameObject);
        }
    }

    public void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }
}
