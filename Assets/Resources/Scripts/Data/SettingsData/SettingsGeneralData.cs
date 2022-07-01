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

        public int ScreenModeIndex { get; private set; }
        public int QualityIndex { get; private set; }
        public bool IsShadowsEnabled { get; private set; }

        public GraphicsData() { }

        public void Initialize()
        {
            ResolutionHeight = SettingsGraphics.CurrentResolution.height;
            ResolutionWidth = SettingsGraphics.CurrentResolution.width;
            RefreshRate = SettingsGraphics.CurrentResolution.refreshRate;

            ScreenModeIndex = SettingsGraphics.CurrentScreenModeIndex;
            QualityIndex = SettingsGraphics.CurrentQualityLevelIndex;
            IsShadowsEnabled = SettingsGraphics.IsShadowsEnabled;
        }
    }

    [System.Serializable]
    public class AudioData
    {
        public float GeneralVolume { get; private set; }
        public float GameSoundsVolume { get; private set; }
        public float AmbienceVolume { get; private set; }

        public AudioData() { }

        public void Initialize()
        {
            GeneralVolume = SettingsAudio.GeneralVolume;
            GameSoundsVolume = SettingsAudio.GameSoundsVolume;
            AmbienceVolume = SettingsAudio.AmbienceVolume;
        }
    }

    [System.Serializable]
    public class UserData
    {
        public string Username { get; private set; }
        public bool IsSensitivitySplitted { get; private set; }
        public float XSensitivity { get; private set; }
        public float YSensitivity { get; private set; }

        public UserData() { }

        public void Initialize()
        {
            Username = SettingsUser.Username;
            IsSensitivitySplitted = SettingsUser.IsSensitivitySplit;
            XSensitivity = SettingsUser.Sensitivity.x;
            YSensitivity = SettingsUser.Sensitivity.y;
        }
    }

    public GraphicsData graphicsData { get; private set; }
    public AudioData audioData { get; private set; }
    public UserData userData { get; private set; }

    public SettingsGeneralData()
    {
        graphicsData = new GraphicsData();
        audioData = new AudioData();
        userData = new UserData();
    }

    public void Initialize()
    {
        graphicsData.Initialize();
        audioData.Initialize();
        userData.Initialize();
    }
}
