using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    public static Credits Instance { get; private set; }

    [SerializeField] private GameObject credits;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        Instance = this;
        exitButton.onClick.AddListener(() => {
            credits.SetActive(false);
        });
    }

    void Start()
    {
        credits.SetActive(false);
    }

    public void ShowCredits()
    {
        credits.SetActive(true);
    }
}
