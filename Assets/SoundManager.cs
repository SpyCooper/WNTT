using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public const string PLAYER_PREFS_GAMEVOLUME = "GameVolume"; 

    [SerializeField] private TextMeshProUGUI sliderText;

    private AudioSource audioSource;
    private float volume;

    // on Awake
    private void Awake()
    {
        // declares this as the singleton
        Instance = this;
        PlayerPrefs.GetFloat(PLAYER_PREFS_GAMEVOLUME, 2f);
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_GAMEVOLUME);
    }

    private void Start()
    {
        if(audioSource != null )
        {
            audioSource.volume = volume / 10;
        }
        
    }

    public void SoundSliderChanged(float value)
    {
        volume = value;
        audioSource.volume = volume/10;
        PlayerPrefs.SetFloat(PLAYER_PREFS_GAMEVOLUME, volume);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public float GetSoundVolume()
    { 
        return volume;
    }
}
