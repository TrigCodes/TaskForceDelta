/***************************************************************
*file: RightHUD.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behavior for RightHUD
*
****************************************************************/
using UnityEngine;
using UnityEngine.UIElements;

public class RightHUD : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement rightHUD;
    private Button machineGunTurretButton, hunterTurretButton, blastTurretButton, wallButton;

    // function: Start
    // purpose: Get all necessary info for gameObject
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        rightHUD = root.Q<VisualElement>("RightHUD");

        // Initialize buttons
        machineGunTurretButton = rightHUD.Q<Button>("MachineGunTurretButton");
        hunterTurretButton = rightHUD.Q<Button>("HunterTurretButton");
        blastTurretButton = rightHUD.Q<Button>("BlastTurretButton");
        wallButton = rightHUD.Q<Button>("WallButton");

        // Set button callbacks with turret prefab names
        machineGunTurretButton.clicked += () => PlaceTurret("MachineGunTurret");
        hunterTurretButton.clicked += () => PlaceTurret("HunterTurret");
        blastTurretButton.clicked += () => PlaceTurret("BlastTurret");
        wallButton.clicked += () => PlaceWall();

        UpdateUI();
        HideHUD();
    }

    // function: UpdateUI
    // purpose: change UI value
    private void UpdateUI()
    {
        // Retrieve costs for each turret type using the FindCostByName method
        int machineGunTurretCost = FindCostByName("MachineGunTurret");
        int hunterTurretCost = FindCostByName("HunterTurret");
        int blastTurretCost = FindCostByName("BlastTurret");

        int wallCount = LevelManager.main.wallCount;

        // Update button texts with the retrieved costs
        machineGunTurretButton.text = $"Machine\nGun\nTurret\n\n{machineGunTurretCost} Scraps";
        hunterTurretButton.text = $"Hunter\nTurret\n\n{hunterTurretCost} Scraps";
        blastTurretButton.text = $"Blast\nTurret\n\n{blastTurretCost} Scraps";

        wallButton.text = $"Walls\n\n{wallCount} Left";
        if (wallCount <= 0)
        {
            wallButton.SetEnabled(false);
        }
    }

    // function: ShowHUD
    // purpose: show right HUD
    public void ShowHUD()
    {
        UpdateUI();
        rightHUD.style.display = DisplayStyle.Flex;
    }
    // function: HideHUD
    // purpose: hide right HUD
    public void HideHUD()
    {
        rightHUD.style.display = DisplayStyle.None;
    }
    // function: PlaceTurret
    // purpose: select turret to place
    private void PlaceTurret(string turretName)
    {
        GameObject turretToPlace = FindTurretByName(turretName);
        if (turretToPlace != null)
        {
            if(PlaceableTile.selectedTile.PlaceTurret(turretToPlace, FindCostByName(turretName)))
            {
                HideHUD(); // Hide the HUD after placing a turret
            }
            else
            {
                FindObjectOfType<Alert>().DisplayAlert($"Not Enough Scraps for {turretName}");
            }

        }
    }
    // function: PlaceWall
    // purpose: select wall to place
    private void PlaceWall()
    {
        PlaceableTile.selectedTile.PlaceWall();
        HideHUD();
    }
    // function: FindTurretByName
    // purpose: get turret from name
    private GameObject FindTurretByName(string name)
    {
        foreach (GameObject turret in LevelManager.main.turretPrefabs)
        {
            if (turret.name == name)
            {
                return turret;
            }
        }
        return null; // Return null if no turret matches the name
    }
    // function: FindCostByName
    // purpose: get turret cost from name
    public int FindCostByName(string name)
    {
        GameObject turret = FindTurretByName(name);
        if (turret != null)
        {
            Turret turretComponent = turret.GetComponent<Turret>();
            if (turretComponent != null)
            {
                return turretComponent.cost;
            }
        }
        return -1; // Return -1 or throw an exception if the turret or component is not found
    }
}