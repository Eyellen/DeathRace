using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class SettingsData
{
    #region GraphicsSettings
    [System.Serializable]
    public static class GraphicsData
    {
        public static int ResolutionHeight { get; private set; }
        public static int ResolutionWidth { get; private set; }
        public static int RefreshRate { get; private set; }

        public static int DisplayModeIndex { get; private set; }
        public static int QualityIndex { get; private set; }
        public static bool IsShadowsEnabled { get; private set; }

        public static void SetSettingsGraphicsData(SettingsGraphics settingsGraphics)
        {
            ResolutionHeight = settingsGraphics.CurrentResolution.height;
            ResolutionWidth = settingsGraphics.CurrentResolution.width;
            RefreshRate = settingsGraphics.CurrentResolution.refreshRate;

            DisplayModeIndex = settingsGraphics.DisplayModeIndex;
            QualityIndex = settingsGraphics.QualityIndex;
            IsShadowsEnabled = settingsGraphics.IsShadowsEnabled;
        }
    }
    #endregion

    #region AudioSettings
    [System.Serializable]
    public static class AudioData
    {
        public static float GeneralVolume { get; private set; }
        public static float GameSoundsVolume { get; private set; }
        public static float AmbienceVolume { get; private set; }

        public static void SetSettingsAudioData(SettingsAudio settingsAudio)
        {
            GeneralVolume = settingsAudio.GeneralVolume;
            GameSoundsVolume = settingsAudio.GameSoundsVolume;
            AmbienceVolume = settingsAudio.AmbienceVolume;
        }
    }
    #endregion

    #region UserSettings
    [System.Serializable]
    public static class UserData
    {
        public static string Username { get; private set; }
        public static float XSensitivity { get; private set; }
        public static float YSensitivity { get; private set; }

        public static void SetSettingsUserData(SettingsUser settingsUser)
        {
            Username = settingsUser.Username;
            XSensitivity = settingsUser.Sensitivity.x;
            YSensitivity = settingsUser.Sensitivity.y;
        }
    }
    #endregion

    //public static void SetSettingsData
    //    (
    //    SettingsGraphics settingsGraphics,
    //    SettingsAudio settingsAudio,
    //    SettingsUser settingsUser
    //    )
    //{
    //    // Graphics settings
    //    ResolutionHeight = settingsGraphics.CurrentResolution.height;
    //    ResolutionWidth = settingsGraphics.CurrentResolution.width;
    //    RefreshRate = settingsGraphics.CurrentResolution.refreshRate;

    //    DisplayModeIndex = settingsGraphics.DisplayModeIndex;
    //    QualityIndex = settingsGraphics.QualityIndex;
    //    IsShadowsEnabled = settingsGraphics.IsShadowsEnabled;

    //    // Audio settings
    //    GeneralVolume = settingsAudio.GeneralVolume;
    //    GameSoundsVolume = settingsAudio.GameSoundsVolume;
    //    AmbienceVolume = settingsAudio.AmbienceVolume;

    //    // User settings
    //    Username = settingsUser.Username;
    //    XSensitivity = settingsUser.Sensitivity.x;
    //    YSensitivity = settingsUser.Sensitivity.y;
    //}
}
