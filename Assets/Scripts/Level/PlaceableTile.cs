/***************************************************************
*file: PlaceableTile.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide bahavior for tiles
*
****************************************************************/
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

    // function: Start
    // purpose: Called before the first frame update to get gameObject necessary info.
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        originalColor = spriteRenderer.color;
    }

    // function: Update
    // purpose: Update is called once per frame to check behavior
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
    // function: OnMouseDown
    // purpose: handling event when player click the mouse on tile
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
    // function: OnMouseEnter
    // purpose: handling event when mouse enter tile
    void OnMouseEnter()
    {
        // If player is aiming, dont distract them by highlighting placeable tiles
        // Dont show highlight over tiles with turrets already placed
        if (!playerAiming && currentTurret == null && selectedTile != this)
        {
            spriteRenderer.color = Color.magenta;
        }
    }
    // function: OnMouseExit
    // purpose: handling event when mouse exit tile
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
    // function: ShowTurretOptions
    // purpose: show turret option when mouse down
    private void ShowTurretOptions()
    {
        RightHUD uiManager = FindObjectOfType<RightHUD>();
        uiManager.ShowHUD();
    }
    // function: PlaceTurret
    // purpose: place turret on tile
    public bool PlaceTurret(GameObject turretPrefab, int cost)
    {
        if (turretPrefab != null && LevelManager.main.SpendScraps(cost))
        {
            currentTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity, transform);
            GetComponent<Collider2D>().enabled = false;
            ResetColor();
            return true;
        }
        else
        {
            return false;
        }
    }
    // function: PlaceWall
    // purpose: place wall on tile
    public void PlaceWall()
    {
        GameObject wallPrefab = LevelManager.main.wallPrefab;
        GameObject tilePrefab = LevelManager.main.tilePrefab;
        if (wallPrefab != null && LevelManager.main.SpendWall() &&
            tilePrefab != null)
        {
            GameObject tile = Instantiate(tilePrefab, transform.position, Quaternion.identity);
            tile.GetComponent<SpriteRenderer>().sprite = LevelManager.main.replaceTilePrefabSprite;

            Instantiate(wallPrefab, transform.position, Quaternion.identity);
            ResetColor();

            Destroy(gameObject);
        }
    }
    // function: ResetColor
    // purpose: reset tile back to original color
    public void ResetColor()
    {
        spriteRenderer.color = originalColor;
    }
}
