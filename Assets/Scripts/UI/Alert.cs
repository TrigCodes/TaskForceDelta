using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class Alert : MonoBehaviour
{
    private VisualElement alertBanner;
    private Label alertLabel;
    private float animationTime = 0.2f; // Time for grow/shrink animation
    private int waitSeconds = 5; // Read time
    private bool isAnimating = false; // To track if an animation is currently playing

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        alertBanner = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("AlertBanner");
        alertLabel = GetComponent<UIDocument>().rootVisualElement.Q<Label>("AlertLabel");

        // Initially hide notification banner
        alertBanner.style.display = DisplayStyle.None;
        alertBanner.transform.scale = Vector3.zero; // Start scaled down to 0
    }

    public void DisplayAlert(string message)
    {
        if (isAnimating)
        {
            StopAllCoroutines(); // Stop any ongoing animations
            alertBanner.transform.scale = Vector3.zero; // Reset scale
            alertBanner.style.display = DisplayStyle.None; // Ensure it is hidden before starting again
        }

        alertLabel.text = message;
        alertBanner.style.display = DisplayStyle.Flex; // Make the banner visible
        StartCoroutine(AnimateAlert());
    }

    private IEnumerator AnimateAlert()
    {
        isAnimating = true; // Set flag to indicate animation is active

        // Grow to slightly larger than normal size
        yield return StartCoroutine(ScaleAlert(Vector3.one * 1.2f, animationTime));
        // Bounce back to normal size
        yield return StartCoroutine(ScaleAlert(Vector3.one, animationTime / 2));
        // Wait for read time
        yield return StartCoroutine(WaitForReadTime());
        // Shrink and hide
        yield return StartCoroutine(ScaleAlert(Vector3.zero, animationTime));

        alertBanner.style.display = DisplayStyle.None; // Hide after animation
        isAnimating = false; // Reset flag as animation ends
    }

    private IEnumerator ScaleAlert(Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 startScale = alertBanner.transform.scale;
        while (time < duration)
        {
            alertBanner.transform.scale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        alertBanner.transform.scale = targetScale; // Ensure it reaches the target scale exactly
    }

    private IEnumerator WaitForReadTime()
    {
        yield return new WaitForSeconds(waitSeconds);
    }
}
