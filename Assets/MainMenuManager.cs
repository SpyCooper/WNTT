using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button playButton;

    void Start()
    {

        exitButton.onClick.AddListener(() => {
            Application.Quit();
        });
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });

        // start music
        // start animations
    }

    void Update()
    {
        
    }
}
