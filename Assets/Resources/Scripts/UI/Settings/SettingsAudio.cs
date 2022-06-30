using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsAudio : MonoBehaviour
{
    private const float _minDecibel = -80f;
    private const float _maxDecibel = 0f;

    [Header("Mixers")]
    [SerializeField] private AudioMixer _generalMixer;
    [SerializeField] private AudioMixer _gameSoundsMixer;
    [SerializeField] private AudioMixer _ambienceMixer;

    [Header("Sliders")]
    [SerializeField] private Slider _generalSlider;
    [SerializeField] private Slider _gameSoundsSlider;
    [SerializeField] private Slider _ambienceSlider;

    public float GeneralVolume { get; private set; }
    public float GameSoundsVolume { get; private set; }
    public float AmbienceVolume { get; private set; }

    public void SetGeneralVolume(float volume)
    {
        GeneralVolume = volume;
        _generalMixer.SetFloat("Volume", FloatToDecibel(volume));
        RefreshOptions();
    }

    public void SetGameSoundsVolume(float volume)
    {
        GameSoundsVolume = volume;
        _gameSoundsMixer.SetFloat("Volume", FloatToDecibel(volume));
        RefreshOptions();
    }

    public void SetAmbienceVolume(float volume)
    {
        AmbienceVolume = volume;
        _ambienceMixer.SetFloat("Volume", FloatToDecibel(volume));
        RefreshOptions();
    }

    private float FloatToDecibel(float volume)
    {
        return Mathf.Lerp(_minDecibel, _maxDecibel, volume);
    }

    private void RefreshOptions()
    {
        _generalSlider.value = GeneralVolume;
        _gameSoundsSlider.value = GameSoundsVolume;
        _ambienceSlider.value = AmbienceVolume;
    }

    private void Start()
    {
        SettingsGeneralData data = SettingsSaveSystem.CachedSave;

        GeneralVolume = data.audioData.GeneralVolume;
        GameSoundsVolume = data.audioData.GameSoundsVolume;
        AmbienceVolume = data.audioData.AmbienceVolume;

        RefreshOptions();
    }
}
