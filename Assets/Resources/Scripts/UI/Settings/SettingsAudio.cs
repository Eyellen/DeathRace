using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsAudio : MonoBehaviour
{
    [SerializeField] private AudioMixer _generalMixer;
    [SerializeField] private AudioMixer _gameSoundsMixer;
    [SerializeField] private AudioMixer _ambienceMixer;

    public float GeneralVolume { get; private set; }
    public float GameSoundsVolume { get; private set; }
    public float AmbienceVolume { get; private set; }

    public void SetGeneralVolume(float volume)
    {
        GeneralVolume = volume;
        _generalMixer.SetFloat("Volume", FloatToDecibel(volume));
    }

    public void SetGameSoundsVolume(float volume)
    {
        GameSoundsVolume = volume;
        _gameSoundsMixer.SetFloat("Volume", FloatToDecibel(volume));
    }

    public void SetAmbienceVolume(float volume)
    {
        AmbienceVolume = volume;
        _ambienceMixer.SetFloat("Volume", FloatToDecibel(volume));
    }

    private float FloatToDecibel(float volume)
    {
        return Mathf.Lerp(-80f, 20f, volume);
    }
}
