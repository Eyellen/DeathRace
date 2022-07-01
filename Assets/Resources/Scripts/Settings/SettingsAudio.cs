using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class SettingsAudio
{
    private const float _minDecibel = -30f;
    private const float _maxDecibel = 0f;
    private const string _keyVolume = "Volume";

    private static AudioMixer s_generalMixer;
    private static AudioMixer s_gameSoundsMixer;
    private static AudioMixer s_ambienceMixer;

    public static void Initialize()
    {
        s_generalMixer = Resources.Load("AudioMixers/General") as AudioMixer;
        s_gameSoundsMixer = Resources.Load("AudioMixers/GameSounds") as AudioMixer;
        s_ambienceMixer = Resources.Load("AudioMixers/Ambience") as AudioMixer;

        //Debug.Log($"General mixer is loaded: {_generalMixer != null}, type: {_generalMixer?.GetType()}");
        //Debug.Log($"GameSounds mixer is loaded: {_gameSoundsMixer != null}, type: {_gameSoundsMixer?.GetType()}");
        //Debug.Log($"Ambience mixer is loaded: {_ambienceMixer != null}, type: {_ambienceMixer?.GetType()}");
    }

    public static void SetDefaultValues()
    {
        SetGeneralVolume(0.5f);
        SetGameSoundsVolume(0.5f);
        SetAmbienceVolume(0.5f);
    }

    public static float GeneralVolume 
    {
        get
        {
            float volume = 0;
            s_generalMixer.GetFloat(_keyVolume, out volume);
            return DecibelToFloat01(volume);
        }
    }
    public static float GameSoundsVolume
    {
        get
        {
            float volume = 0;
            s_gameSoundsMixer.GetFloat(_keyVolume, out volume);
            return DecibelToFloat01(volume);
        }
    }
    public static float AmbienceVolume
    {
        get
        {
            float volume = 0;
            s_ambienceMixer.GetFloat(_keyVolume, out volume);
            return DecibelToFloat01(volume);
        }
    }

    public static void SetGeneralVolume(float volume)
    {
        s_generalMixer.SetFloat(_keyVolume, Float01ToDecibel(volume));
    }

    public static void SetGameSoundsVolume(float volume)
    {
        s_gameSoundsMixer.SetFloat(_keyVolume, Float01ToDecibel(volume));
    }

    public static void SetAmbienceVolume(float volume)
    {
        s_ambienceMixer.SetFloat(_keyVolume, Float01ToDecibel(volume));
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
