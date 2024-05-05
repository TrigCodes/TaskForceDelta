/***************************************************************
*file: BottomHUD.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behavior for BottomHUD
*
****************************************************************/
using System.Collections;
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
    private Coroutine shakeCoroutine;
    private Vector3 originalShakePosition;

    // function: Start
    // purpose: Get all necessary info for gameObject
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
    // function: SetCurrentTurret
    // purpose: set selected turret to current
    public void SetCurrentTurret(Turret turret)
    {
        currentTurret = turret;
        UpdateUI();
        bottomHUD.style.display = DisplayStyle.Flex;  // Show the BottomHUD
        leftHUD.style.display = DisplayStyle.Flex;
    }
    // function: UpdateUI
    // purpose: change UI value
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
    // function: UpdateTurretHP
    // purpose: change turret UI hp
    public void UpdateTurretHP(Turret turret, int newHealth, bool tookHit = false)
    {
        if (turret == currentTurret)
        {
            turretHPProgressBar.highValue = currentTurret.GetComponent<Health>().GetMaxHealth();
            turretHPProgressBar.value = newHealth;

            if (tookHit)
            {
                if (shakeCoroutine != null)
                {
                    StopCoroutine(shakeCoroutine);
                    turretHPProgressBar.transform.position = originalShakePosition; // Reset position on stop
                }
                shakeCoroutine = StartCoroutine(ShakeHealthBar());
            }
        }
    }
    // function: ShakeHealthBar
    // purpose: shake health bar whehn took hit
    private IEnumerator ShakeHealthBar()
    {
        float shakeDuration = 0.4f; // duration of the shake in seconds
        float shakeMagnitude = 8f; // magnitude of the shake

        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            turretHPProgressBar.transform.position = new Vector3(originalShakePosition.x + x,
                                                                 originalShakePosition.y + y,
                                                                 originalShakePosition.z);

            elapsed += Time.deltaTime;
            yield return null; // wait for the next frame
        }

        // Return the progress bar to its original position
        turretHPProgressBar.transform.position = originalShakePosition;
    }
    // function: HideHUD
    // purpose: hide bottom HUD
    public void HideHUD()
    {
        bottomHUD.style.display = DisplayStyle.None;  // Hide the BottomHUD
        leftHUD.style.display = DisplayStyle.None;
    }
    // function: UpgradeTurret
    // purpose: upgrade turret 
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