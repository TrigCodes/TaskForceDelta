/***************************************************************
*file: MainMenuButtons.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide behavior for MainMenuButtons
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    private UIDocument _document;
    private Button _startButton;
    private Button _quitGame;

    // function: Awake
    // purpose: called before everything to get necessary info
    private void Awake()
    {

        _document = GetComponent<UIDocument>();
        _startButton = _document.rootVisualElement.Q("StartButton") as Button;
        _startButton.RegisterCallback<ClickEvent>(StartGame);

        _quitGame = _document.rootVisualElement.Q("QuitButton") as Button;
        _quitGame.RegisterCallback<ClickEvent>(QuitGame);


    }
    // function: StartGame
    // purpose: handling when player start the game
    private void StartGame(ClickEvent clickEvent)
    {
        SceneManager.LoadScene("LevelOne");
        Time.timeScale = 1;
    }
    // function: QuitGame
    // purpose: handling when player quit the game
    private void QuitGame(ClickEvent clickEvent)
    {
        Application.Quit();
    }

}