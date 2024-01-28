using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    private GameManager.GameState previousGameState;
    [SerializeField] private GameObject settingsMenu;

    private void Awake()
    {
        settingsButton.onClick.AddListener(() => {
            SettingsButtonPressed();
        });
        resumeButton.onClick.AddListener(() => {
            CloseSettingsMenu();
        });
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start()
    {
        CloseSettingsMenu();
    }

    private void SettingsButtonPressed()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.Paused)
        {
            CloseSettingsMenu();
        }
        else
        {
            OpenSettingsMenu();
        }
    }

    private void CloseSettingsMenu()
    {
        if(GameManager.Instance.gameState != GameManager.GameState.InfoPhase)
        {
            GameManager.Instance.gameState = previousGameState;
        }
        
        settingsMenu.SetActive(false);
    }

    private void OpenSettingsMenu()
    {
        previousGameState = GameManager.Instance.gameState;
        GameManager.Instance.gameState = GameManager.GameState.Paused;
        settingsMenu.SetActive(true);
    }
}
