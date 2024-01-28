using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameSoundSlider : MonoBehaviour
{
    public const string PLAYER_PREFS_GAMEVOLUME = "GameVolume";

    // all the sound setting sliders are the same
    [SerializeField] private Slider slider;

    // on Start
    private void Start()
    {
        OnSliderChanged(SoundManager.Instance.GetSoundVolume());
        
    }

    public void OnSliderChanged(float value)
    {
        // save the value based on the value move on the slider
        slider.value = value;

        SoundManager.Instance.SoundSliderChanged(value);
    }
}
