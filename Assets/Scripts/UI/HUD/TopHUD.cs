/***************************************************************
*file: TopHUD.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behavior for TopHUD
*
****************************************************************/
using UnityEngine;
using UnityEngine.UIElements;

public class TopHUD : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label scrapsLabel;
    private Label waveLabel;
    private ProgressBar coreHealthBar;
    private VisualElement root;

    // function: Start
    // purpose: Get all necessary info for gameObject
    void Start()
    {
        // Make sure there is a UI Document attached to the same Game Object
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        scrapsLabel = root.Q<Label>("ScrapsLabel");
        waveLabel = root.Q<Label>("WaveLabel");
        coreHealthBar = root.Q<ProgressBar>("CoreHealthBar");

        // Initial update
        UpdateScrapsDisplay(LevelManager.main.TotalScraps);

        coreHealthBar.highValue = LevelManager.main.core.GetComponent<Health>().GetMaxHealth();
        UpdateCoreHPDisplay(LevelManager.main.core.GetComponent<Health>().GetCurrentHealth());
    }

    // function: UpdateScrapsDisplay
    // purpose: update scraps display at top HUD
    public void UpdateScrapsDisplay(int newScrapsAmount)
    {
        if (scrapsLabel != null)
        {
            scrapsLabel.text = $"Scraps: {newScrapsAmount}";
        }
    }
    // function: UpdateCoreHPDisplay
    // pupose: update core HP at top HUD
    public void UpdateCoreHPDisplay(int newCoreHP)
    {
        if (coreHealthBar != null)
        {
            coreHealthBar.value = newCoreHP;
        }
    }
    // function: UpdateWaveDisplay
    // purpose: update wave info
    public void UpdateWaveDisplay(int currentWave, int totalWaves)
    {
        if (waveLabel != null)
        {
            waveLabel.text = $"Wave {currentWave} / {totalWaves}";
        }
    }
}
