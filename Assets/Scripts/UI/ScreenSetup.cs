using UnityEngine;

public class ScreenSetup : MonoBehaviour
{
    void Start()
    {
        // Set the resolution to 1920x1080 and set it to windowed mode
        Screen.SetResolution(1920, 1080, false);
    }
}
