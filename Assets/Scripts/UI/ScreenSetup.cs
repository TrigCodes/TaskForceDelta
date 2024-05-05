/***************************************************************
*file: ScreenSetup.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behavior for ScreenSetup
*
****************************************************************/
using UnityEngine;

public class ScreenSetup : MonoBehaviour
{
    // function: Start
    // purpose: set up screen resolution
    void Start()
    {
        // Set the resolution to 1920x1080 and set it to windowed mode
        Screen.SetResolution(1920, 1080, false);
    }
}
