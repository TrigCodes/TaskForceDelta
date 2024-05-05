/***************************************************************
*file: PauseMenu.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behavior for PauseMenu
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private VisualElement pauseMenu;
    bool gamePaused = false;

    // function: Start
    // purpose: Start is called before the first frame update to get and set necessary info
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        pauseMenu = root.Q<VisualElement>("PauseMenu");
        root.Q<Button>("ResumeButton").clicked += () => ResumeGame();
        root.Q<Button>("MainMenuButton").clicked += () => MainMenu();

        Slider masterVolumeSlider = root.Q<Slider>("MasterVolumeSlider");
        Slider soundVolumeSlider = root.Q<Slider>("SoundVolumeSlider");
        Slider musicVolumeSlider = root.Q<Slider>("MusicVolumeSlider");

        // Set initial slider values
        masterVolumeSlider.value = 0.75f;
        soundVolumeSlider.value = 0.75f;
        musicVolumeSlider.value = 0.75f;

        masterVolumeSlider.RegisterValueChangedCallback(evt => AudioManager.main.SetMasterVolume(evt.newValue));
        soundVolumeSlider.RegisterValueChangedCallback(evt => AudioManager.main.SetSoundFXVolume(evt.newValue));
        musicVolumeSlider.RegisterValueChangedCallback(evt => AudioManager.main.SetMusicVolume(evt.newValue));
    }
    // function: Update
    // purpose: Update is called once per frame to check if player pause
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    // function: PauseGame
    // purpose: pause the game
    private void PauseGame()
    {
        Time.timeScale = 0;
        gamePaused = true;
        pauseMenu.style.display = DisplayStyle.Flex;
    }
    // function: ResumeGame
    // purpose: resume the game
    private void ResumeGame()
    {
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.style.display = DisplayStyle.None;
    }
    // function: MainMenu
    // purpose: return to main menu
    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
