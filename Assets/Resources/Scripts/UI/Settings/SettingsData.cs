using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsData
{
    #region GraphicsSettings
    public static int ResolutionHeight { get; private set; }
    public static int ResolutionWidth { get; private set; }
    public static int RefreshRate { get; private set; }

    public static int DisplayModeIndex { get; private set; }
    public static int QualityIndex { get; private set; }
    public static bool IsShadowsEnabled { get; private set; }
    #endregion

    #region AudioSettings
    public static float GeneralVolume { get; private set; }
    public static float GameSoundsVolume { get; private set; }
    public static float AmbienceVolume { get; private set; }
    #endregion

    #region UserSettings
    public static string Username { get; private set; }
    public static float XSensitivity { get; private set; }
    public static float YSensitivity { get; private set; }
    #endregion

    public static void SetSettingsData
        (
        SettingsGraphics settingsGraphics,
        SettingsAudio settingsAudio,
        SettingsUser settingsUser
        )
    {
        // Graphics settings
        ResolutionHeight = settingsGraphics.CurrentResolution.height;
        ResolutionWidth = settingsGraphics.CurrentResolution.width;
        RefreshRate = settingsGraphics.CurrentResolution.refreshRate;

        DisplayModeIndex = settingsGraphics.DisplayModeIndex;
        QualityIndex = settingsGraphics.QualityIndex;
        IsShadowsEnabled = settingsGraphics.IsShadowsEnabled;

        // Audio settings
        GeneralVolume = settingsAudio.GeneralVolume;
        GameSoundsVolume = settingsAudio.GameSoundsVolume;
        AmbienceVolume = settingsAudio.AmbienceVolume;

        // User settings
        Username = settingsUser.Username;
        XSensitivity = settingsUser.Sensitivity.x;
        YSensitivity = settingsUser.Sensitivity.y;
    }
}
