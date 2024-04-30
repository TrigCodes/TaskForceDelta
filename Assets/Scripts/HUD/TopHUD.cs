using UnityEngine;
using UnityEngine.UIElements;

public class TopHUD : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label scrapsLabel;
    private VisualElement root;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure there is a UI Document attached to the same Game Object
        uiDocument = GetComponent<UIDocument>();

        root = uiDocument.rootVisualElement;

        scrapsLabel = root.Q<Label>("ScrapsLabel");

        UpdateScrapsDisplay(LevelManager.main.TotalScraps); // Initial update
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
}
