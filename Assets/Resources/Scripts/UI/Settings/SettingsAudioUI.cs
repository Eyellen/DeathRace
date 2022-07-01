using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsAudioUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider _generalSlider;
    [SerializeField] private Slider _gameSoundsSlider;
    [SerializeField] private Slider _ambienceSlider;

    private void Start()
    {
        RefreshOptions();
    }

    public void SetGeneralVolume(float volume)
    {
        SettingsAudio.SetGeneralVolume(volume);
        RefreshOptions();
    }

    public void SetGameSoundsVolume(float volume)
    {
        SettingsAudio.SetGameSoundsVolume(volume);
        RefreshOptions();
    }

    public void SetAmbienceVolume(float volume)
    {
        SettingsAudio.SetAmbienceVolume(volume);
        RefreshOptions();
    }

    private void RefreshOptions()
    {
        _generalSlider.value = SettingsAudio.GeneralVolume;
        _gameSoundsSlider.value = SettingsAudio.GameSoundsVolume;
        _ambienceSlider.value = SettingsAudio.AmbienceVolume;
    }
}
