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
        SettingsGraphics.InitializeSettings();
        SettingsGraphics.SetResolution(_loadedSave.graphicsData.ResolutionWidth, _loadedSave.graphicsData.ResolutionHeight);
        SettingsGraphics.SetScreenMode(_loadedSave.graphicsData.ScreenModeIndex);
        SettingsGraphics.SetQualityLevel(_loadedSave.graphicsData.QualityIndex);
        SettingsGraphics.SetShadowsEnabled(_loadedSave.graphicsData.IsShadowsEnabled);

        _generalMixer.SetFloat("Volume", Float01ToDecibel(_loadedSave.audioData.GeneralVolume));
        _gameSoundsMixer.SetFloat("Volume", Float01ToDecibel(_loadedSave.audioData.GameSoundsVolume));
        _ambienceMixer.SetFloat("Volume", Float01ToDecibel(_loadedSave.audioData.AmbienceVolume));
    }

    private float Float01ToDecibel(float value)
    {
        return Mathf.Lerp(-80, 20, value);
    }
}
