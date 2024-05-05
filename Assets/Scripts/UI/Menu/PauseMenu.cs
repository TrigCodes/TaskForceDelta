using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private VisualElement pauseMenu;
    bool gamePaused = false;

    // Start is called before the first frame update
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

    // Update is called once per frame
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

    private void PauseGame()
    {
        Time.timeScale = 0;
        gamePaused = true;
        pauseMenu.style.display = DisplayStyle.Flex;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.style.display = DisplayStyle.None;
    }

    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
