using UnityEngine;
using UnityEngine.UIElements;

public class TopHUD : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label scrapsLabel;
    private ProgressBar coreHealthBar;
    private VisualElement root;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure there is a UI Document attached to the same Game Object
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        scrapsLabel = root.Q<Label>("ScrapsLabel");
        coreHealthBar = root.Q<ProgressBar>("CoreHealthBar");

        // Initial update
        UpdateScrapsDisplay(LevelManager.main.TotalScraps);

        coreHealthBar.highValue = LevelManager.main.core.GetComponent<Health>().GetMaxHealth();
        UpdateCoreHPDisplay(LevelManager.main.core.GetComponent<Health>().GetCurrentHealth());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScrapsDisplay(int newScrapsAmount)
    {
        if (scrapsLabel != null)
        {
            scrapsLabel.text = $"Scraps: {newScrapsAmount}";
        }
    }

    public void UpdateCoreHPDisplay(int newCoreHP)
    {
        if (coreHealthBar != null)
        {
            coreHealthBar.value = newCoreHP;
        }
    }
}
