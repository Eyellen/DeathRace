using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class SettingsAudio
{
    private const float _minDecibel = -30f;
    private const float _maxDecibel = 0f;
    private const string _keyVolume = "Volume";

    private static AudioMixer _generalMixer;
    private static AudioMixer _gameSoundsMixer;
    private static AudioMixer _ambienceMixer;

    public static void Initialize()
    {
        _generalMixer = Resources.Load("AudioMixers/General") as AudioMixer;
        _gameSoundsMixer = Resources.Load("AudioMixers/GameSounds") as AudioMixer;
        _ambienceMixer = Resources.Load("AudioMixers/Ambience") as AudioMixer;

        //Debug.Log($"General mixer is loaded: {_generalMixer != null}, type: {_generalMixer?.GetType()}");
        //Debug.Log($"GameSounds mixer is loaded: {_gameSoundsMixer != null}, type: {_gameSoundsMixer?.GetType()}");
        //Debug.Log($"Ambience mixer is loaded: {_ambienceMixer != null}, type: {_ambienceMixer?.GetType()}");
    }

    public static float GeneralVolume 
    {
        get
        {
            float volume = 0;
            _generalMixer.GetFloat(_keyVolume, out volume);
            return DecibelToFloat01(volume);
        }
    }
    public static float GameSoundsVolume
    {
        get
        {
            float volume = 0;
            _gameSoundsMixer.GetFloat(_keyVolume, out volume);
            return DecibelToFloat01(volume);
        }
    }
    public static float AmbienceVolume
    {
        get
        {
            float volume = 0;
            _ambienceMixer.GetFloat(_keyVolume, out volume);
            return DecibelToFloat01(volume);
        }
    }

    public static void SetGeneralVolume(float volume)
    {
        _generalMixer.SetFloat(_keyVolume, Float01ToDecibel(volume));
    }

    public static void SetGameSoundsVolume(float volume)
    {
        _gameSoundsMixer.SetFloat(_keyVolume, Float01ToDecibel(volume));
    }

    public static void SetAmbienceVolume(float volume)
    {
        _ambienceMixer.SetFloat(_keyVolume, Float01ToDecibel(volume));
    }

    private static float Float01ToDecibel(float value)
    {
        if (value <= 0.01) return -80;

        return Mathf.Lerp(_minDecibel, _maxDecibel, value);
    }

    private static float DecibelToFloat01(float decibel)
    {
        decibel = Mathf.Clamp(decibel, _minDecibel, _maxDecibel);

        float absoluteMax = _maxDecibel + Mathf.Abs(_minDecibel);
        float absoluteDecibel = decibel + Mathf.Abs(_minDecibel);

        return absoluteDecibel / absoluteMax;
    }
}
