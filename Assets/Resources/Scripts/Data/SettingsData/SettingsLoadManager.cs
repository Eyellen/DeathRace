using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[DisallowMultipleComponent]
public class SettingsLoadManager : MonoBehaviour
{
    private SettingsGeneralData _loadedSave;

    [SerializeField] private AudioMixer _generalMixer;
    [SerializeField] private AudioMixer _gameSoundsMixer;
    [SerializeField] private AudioMixer _ambienceMixer;

    private void Start()
    {
        _loadedSave = SettingsSaveSystem.Load();

        ApplyLoadedSettings();
    }

    private void ApplyLoadedSettings()
    {
        SettingsGraphics.Initialize();
        SettingsGraphics.SetResolution(_loadedSave.graphicsData.ResolutionWidth, _loadedSave.graphicsData.ResolutionHeight);
        SettingsGraphics.SetScreenMode(_loadedSave.graphicsData.ScreenModeIndex);
        SettingsGraphics.SetQualityLevel(_loadedSave.graphicsData.QualityIndex);
        SettingsGraphics.SetShadowsEnabled(_loadedSave.graphicsData.IsShadowsEnabled);

        SettingsAudio.Initialize();
        SettingsAudio.SetGeneralVolume(_loadedSave.audioData.GeneralVolume);
        SettingsAudio.SetGameSoundsVolume(_loadedSave.audioData.GameSoundsVolume);
        SettingsAudio.SetAmbienceVolume(_loadedSave.audioData.AmbienceVolume);
    }
}
