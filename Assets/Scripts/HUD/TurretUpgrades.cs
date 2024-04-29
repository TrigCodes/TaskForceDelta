using UnityEngine;
using UnityEngine.UIElements;

public class TurretUpgrades : MonoBehaviour
{
    public UIDocument uiDocument;
    private VisualElement root;
    private VisualElement bottomHUD;
    private Turret currentTurret;

    private Button damageUpgradeButton, fireRateUpgradeButton, shieldUpgradeButton;
    private ProgressBar damageLevelProgressBar, fireRateLevelProgressBar, shieldLevelProgressBar;

    void Start()
    {
        root = uiDocument.rootVisualElement;

        // Access the BottomHUD panel
        bottomHUD = root.Q<VisualElement>("BottomHUD");

        // Get buttons
        damageUpgradeButton = bottomHUD.Q<Button>("DamageUpgradeButton");
        fireRateUpgradeButton = bottomHUD.Q<Button>("FireRateUpgradeButton");
        shieldUpgradeButton = bottomHUD.Q<Button>("ShieldUpgradeButton");

        // Get progress bars within the buttons
        damageLevelProgressBar = damageUpgradeButton.Q<ProgressBar>("LevelIndicator");
        fireRateLevelProgressBar = fireRateUpgradeButton.Q<ProgressBar>("LevelIndicator");
        shieldLevelProgressBar = shieldUpgradeButton.Q<ProgressBar>("LevelIndicator");

        // Assign callbacks
        damageUpgradeButton.clicked += () => UpgradeTurret("damage");
        fireRateUpgradeButton.clicked += () => UpgradeTurret("fireRate");
        shieldUpgradeButton.clicked += () => UpgradeTurret("shield");

        // Initially hide the BottomHUD panel
        bottomHUD.style.display = DisplayStyle.None;
    }

    public void SetCurrentTurret(Turret turret)
    {
        currentTurret = turret;
        UpdateUI();
        bottomHUD.style.display = DisplayStyle.Flex;  // Show the BottomHUD
    }

    private void UpdateUI()
    {
        // Update progress bars values
        damageLevelProgressBar.value = currentTurret.damageLevel;
        fireRateLevelProgressBar.value = currentTurret.fireRateLevel;
        shieldLevelProgressBar.value = currentTurret.shieldLevel;

        // Control buttons enabled state
        damageUpgradeButton.SetEnabled(currentTurret.damageLevel < Turret.MaxLevel);
        fireRateUpgradeButton.SetEnabled(currentTurret.fireRateLevel < Turret.MaxLevel);
        shieldUpgradeButton.SetEnabled(currentTurret.shieldLevel < Turret.MaxLevel);
    }

    public void HideHUD()
    {
        bottomHUD.style.display = DisplayStyle.None;  // Hide the BottomHUD
    }

    private void UpgradeTurret(string upgradeType)
    {
        if (currentTurret == null) return;

        switch (upgradeType)
        {
            case "damage":
                currentTurret.UpgradeDamage();
                break;
            case "fireRate":
                currentTurret.UpgradeFireRate();
                break;
            case "shield":
                currentTurret.UpgradeShield();
                break;
        }

        UpdateUI();
    }
}