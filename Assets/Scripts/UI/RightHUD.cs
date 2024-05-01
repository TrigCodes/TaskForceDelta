using UnityEngine;
using UnityEngine.UIElements;

public class RightHUD : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement rightHUD;
    private Button machineGunTurretButton, hunterTurretButton, blastTurretButton, wallButton;

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

    private void UpdateUI()
    {
        // Retrieve costs for each turret type using the FindCostByName method
        int machineGunTurretCost = FindCostByName("MachineGunTurret");
        int hunterTurretCost = FindCostByName("HunterTurret");
        int blastTurretCost = FindCostByName("BlastTurret");

        int wallCount = LevelManager.main.wallCount;

        // Update button texts with the retrieved costs
        machineGunTurretButton.text = $"Machine Gun \nTurret\n\n{machineGunTurretCost} Scraps";
        hunterTurretButton.text = $"Hunter Turret\n\n{hunterTurretCost} Scraps";
        blastTurretButton.text = $"Blast Turret\n\n{blastTurretCost} Scraps";

        wallButton.text = $"Walls\n\n{wallCount} Available";
        if (wallCount <= 0)
        {
            wallButton.SetEnabled(false);
        }
    }

    public void ShowHUD()
    {
        UpdateUI();
        rightHUD.style.display = DisplayStyle.Flex;;
    }

    public void HideHUD()
    {
        rightHUD.style.display = DisplayStyle.None;
    }

    private void PlaceTurret(string turretName)
    {
        GameObject turretToPlace = FindTurretByName(turretName);
        if (turretToPlace != null)
        {
            PlaceableTile.selectedTile.PlaceTurret(turretToPlace, FindCostByName(turretName));
            HideHUD(); // Hide the HUD after placing a turret
        }
    }

    private void PlaceWall()
    {
        PlaceableTile.selectedTile.PlaceWall();
        HideHUD();
    }

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