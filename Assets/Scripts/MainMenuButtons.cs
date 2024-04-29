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

    private void Awake()
    {

        _document = GetComponent<UIDocument>();
        _startButton = _document.rootVisualElement.Q("StartButton") as Button;
        _startButton.RegisterCallback<ClickEvent>(StartGame);

        _quitGame = _document.rootVisualElement.Q("QuitGame") as Button;
        _quitGame.RegisterCallback<ClickEvent>(QuitGame);


    }

    private void StartGame(ClickEvent clickEvent)
    {
        Debug.Log("Start button pressed");
        SceneManager.LoadScene("InGame");

    }

    private void QuitGame(ClickEvent clickEvent)
    {
        Debug.Log("Quit game clicked");
        Application.Quit();
    }
    
}
