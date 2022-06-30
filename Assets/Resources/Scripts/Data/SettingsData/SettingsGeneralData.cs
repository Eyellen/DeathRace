using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class designed to save settings data
/// </summary>
[System.Serializable]
public class SettingsGeneralData
{
    [System.Serializable]
    public class GraphicsData
    {
        public int ResolutionHeight { get; private set; }
        public int ResolutionWidth { get; private set; }
        public int RefreshRate { get; private set; }

        public int ResolutionIndex { get; private set; }
        public int DisplayModeIndex { get; private set; }
        public int QualityIndex { get; private set; }
        public bool IsShadowsEnabled { get; private set; }

        public GraphicsData(SettingsGraphicsUI settingsGraphics)
        {
            ResolutionHeight = settingsGraphics.CurrentResolution.height;
            ResolutionWidth = settingsGraphics.CurrentResolution.width;
            RefreshRate = settingsGraphics.CurrentResolution.refreshRate;

            ResolutionIndex = settingsGraphics.ResolutionIndex;
            DisplayModeIndex = settingsGraphics.DisplayModeIndex;
            QualityIndex = settingsGraphics.QualityIndex;
            IsShadowsEnabled = settingsGraphics.IsShadowsEnabled;
        }
    }

    [System.Serializable]
    public class AudioData
    {
        public float GeneralVolume { get; private set; }
        public float GameSoundsVolume { get; private set; }
        public float AmbienceVolume { get; private set; }

        public AudioData(SettingsAudioUI settingsAudio)
        {
            GeneralVolume = settingsAudio.GeneralVolume;
            GameSoundsVolume = settingsAudio.GameSoundsVolume;
            AmbienceVolume = settingsAudio.AmbienceVolume;
        }
    }

    [System.Serializable]
    public class UserData
    {
        public string Username { get; private set; }
        public bool IsSensitivitySplitted { get; private set; }
        public float XSensitivity { get; private set; }
        public float YSensitivity { get; private set; }

        public UserData(SettingsUserUI settingsUser)
        {
            Username = settingsUser.Username;
            IsSensitivitySplitted = settingsUser.IsSensitivitySplitted;
            XSensitivity = settingsUser.Sensitivity.x;
            YSensitivity = settingsUser.Sensitivity.y;
        }
    }

    public GraphicsData graphicsData { get; private set; }
    public AudioData audioData { get; private set; }
    public UserData userData { get; private set; }

    public SettingsGeneralData(SettingsGeneralUI settingsGeneral)
    {
        graphicsData = new GraphicsData(settingsGeneral.SettingsGraphics);
        audioData = new AudioData(settingsGeneral.SettingsAudio);
        userData = new UserData(settingsGeneral.SettingsUser);
    }
}
