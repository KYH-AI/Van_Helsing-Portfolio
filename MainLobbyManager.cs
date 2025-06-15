using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainLobbyManager : MonoBehaviour
{
    private const string START_BUTTON = "StartButton";
    private const string EXIT_BUTTON = "ExitButton";
    private const string GAME_SCENE = "GameScene";
    public void ButtonClickEvent(Button button)
    {
        switch(button.name)
        {
            case START_BUTTON: SceneManager.LoadScene(GAME_SCENE); break;
            case EXIT_BUTTON: Application.Quit(); break;
        }
    }
}
