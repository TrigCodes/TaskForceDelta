using UnityEngine;
using UnityEngine.UIElements;

public class UpgradesHUD : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    private VisualElement bottomHUD;
    private Turret currentTurret;

    private Button damageUpgradeButton, fireRateUpgradeButton, shieldUpgradeButton;
    private ProgressBar damageLevelProgressBar, fireRateLevelProgressBar, shieldLevelProgressBar;

    void Start()
    {
        // Make sure there is a UI Document attached to the same Game Object
        uiDocument = GetComponent<UIDocument>();

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

        damageLevelProgressBar.highValue = currentTurret.damageMaxLevel;
        fireRateLevelProgressBar.highValue = currentTurret.fireRateMaxLevel;
        shieldLevelProgressBar.highValue = currentTurret.shieldMaxLevel;

        damageLevelProgressBar.title = currentTurret.damageLevel.ToString();
        fireRateLevelProgressBar.title = currentTurret.fireRateLevel.ToString();
        shieldLevelProgressBar.title = currentTurret.shieldLevel.ToString();

        // Disable button if max level reached
        damageUpgradeButton.SetEnabled(currentTurret.damageLevel < currentTurret.damageMaxLevel);
        fireRateUpgradeButton.SetEnabled(currentTurret.fireRateLevel < currentTurret.fireRateMaxLevel);
        shieldUpgradeButton.SetEnabled(currentTurret.shieldLevel < currentTurret.shieldMaxLevel);
    }

    public void HideHUD()
    {
        bottomHUD.style.display = DisplayStyle.None;  // Hide the BottomHUD
    }

    private void UpgradeTurret(string upgradeType)
    {
        if (currentTurret == null) return;

        bool upgradePerformed = false;
        switch (upgradeType)
        {
            case "damage":
                upgradePerformed = currentTurret.UpgradeDamage();
                break;
            case "fireRate":
                upgradePerformed = currentTurret.UpgradeFireRate();
                break;
            case "shield":
                upgradePerformed = currentTurret.UpgradeShield();
                break;
        }

        if (upgradePerformed)
        {
            UpdateUI();
        }
        else
        {
            // TODO: Show a message that there are not enough scraps
        }
    }
}