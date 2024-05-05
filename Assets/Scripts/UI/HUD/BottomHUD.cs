using UnityEngine;
using UnityEngine.UIElements;

public class BottomHUD : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement root;
    public VisualElement bottomHUD;
    public VisualElement leftHUD;
    private Turret currentTurret;

    private Button damageUpgradeButton, 
                   fireRateUpgradeButton, 
                   shieldUpgradeButton, 
                   specialUpgradeButton;
    private ProgressBar damageLevelProgressBar, 
                        fireRateLevelProgressBar, 
                        shieldLevelProgressBar,
                        turretHPProgressBar;
    private Label specialUpgradeInfoLabel;

    void Start()
    {
        // Make sure there is a UI Document attached to the same Game Object
        uiDocument = GetComponent<UIDocument>();

        root = uiDocument.rootVisualElement;

        // Access the BottomHUD panel
        bottomHUD = root.Q<VisualElement>("BottomHUD");
        leftHUD = root.Q<VisualElement>("LeftHUD");

        // Get buttons
        damageUpgradeButton = bottomHUD.Q<Button>("DamageUpgradeButton");
        fireRateUpgradeButton = bottomHUD.Q<Button>("FireRateUpgradeButton");
        shieldUpgradeButton = bottomHUD.Q<Button>("ShieldUpgradeButton");
        specialUpgradeButton = bottomHUD.Q<Button>("SpecialUpgradeButton");

        // Get progress bars within the buttons
        damageLevelProgressBar = damageUpgradeButton.Q<ProgressBar>("LevelIndicator");
        fireRateLevelProgressBar = fireRateUpgradeButton.Q<ProgressBar>("LevelIndicator");
        shieldLevelProgressBar = shieldUpgradeButton.Q<ProgressBar>("LevelIndicator");
        turretHPProgressBar = root.Q<ProgressBar>("TurretHealthBar");

        specialUpgradeInfoLabel = specialUpgradeButton.Q<Label>("SpecialUpgradeText");

        // Assign callbacks
        damageUpgradeButton.clicked += () => UpgradeTurret("damage");
        fireRateUpgradeButton.clicked += () => UpgradeTurret("fireRate");
        shieldUpgradeButton.clicked += () => UpgradeTurret("shield");
        specialUpgradeButton.clicked += () => UpgradeTurret("special");

        // Initially hide the BottomHUD panel
        bottomHUD.style.display = DisplayStyle.None;
        leftHUD.style.display = DisplayStyle.None;
    }

    public void SetCurrentTurret(Turret turret)
    {
        currentTurret = turret;
        UpdateUI();
        bottomHUD.style.display = DisplayStyle.Flex;  // Show the BottomHUD
        leftHUD.style.display = DisplayStyle.Flex;
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

        UpdateTurretHP(currentTurret, currentTurret.GetComponent<Health>().GetCurrentHealth());

        // Disable button & set button text if max level reached
        // Damage
        if (currentTurret.damageLevel < currentTurret.damageMaxLevel)
        {
            damageUpgradeButton.SetEnabled(true);
            damageUpgradeButton.text = $"\n\nDamage Upgrade\n{currentTurret.costPerDamageUpgrade} Scraps";
        }
        else
        {
            damageUpgradeButton.SetEnabled(false);
            damageUpgradeButton.text = $"\n\nDamage Upgrade\nFully Upgraded!";
        }
        // Fire Rate
        if (currentTurret.fireRateLevel < currentTurret.fireRateMaxLevel)
        {
            fireRateUpgradeButton.SetEnabled(true);
            fireRateUpgradeButton.text = $"\n\nFire Rate Upgrade\n{currentTurret.costPerFireRateUpgrade} Scraps";
        }
        else
        {
            fireRateUpgradeButton.SetEnabled(false);
            fireRateUpgradeButton.text = $"\n\nFire Rate Upgrade\nFully Upgraded!";
        }
        // Shield
        if (currentTurret.shieldLevel < currentTurret.shieldMaxLevel)
        {
            shieldUpgradeButton.SetEnabled(true);
            shieldUpgradeButton.text = $"\n\nShield Upgrade\n{currentTurret.costPerShieldUpgrade} Scraps";
        }
        else
        {
            shieldUpgradeButton.SetEnabled(false);
            shieldUpgradeButton.text = $"\n\nShield Upgrade\nFully Upgraded!";
        }
        // Special
        if (!currentTurret.GetSpecialUpgradeDone())
        {
            specialUpgradeButton.SetEnabled(true);
            specialUpgradeButton.text = $"\nSpecial Upgrade\n{currentTurret.GetSpecialUpgradeCost()} Scraps";
            specialUpgradeInfoLabel.text = currentTurret.GetSpecialInfoText();
        }
        else
        {
            specialUpgradeButton.SetEnabled(false);
            specialUpgradeButton.text = $"\nSpecial Upgrade\nSpecial Upgrade Unlocked!";
        }
    }

    public void UpdateTurretHP(Turret turret, int newHealth)
    {
        if (turret == currentTurret)
        {
            turretHPProgressBar.highValue = currentTurret.GetComponent<Health>().GetMaxHealth();
            turretHPProgressBar.value = newHealth;
        }
    }

    public void HideHUD()
    {
        bottomHUD.style.display = DisplayStyle.None;  // Hide the BottomHUD
        leftHUD.style.display = DisplayStyle.None;
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
            case "special":
                upgradePerformed = currentTurret.UpgradeSpecial();
                if (upgradePerformed)
                {
                    // Disable button
                    specialUpgradeButton.SetEnabled(false);
                }
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