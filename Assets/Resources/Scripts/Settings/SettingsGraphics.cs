using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResolution
{
    public int Width;
    public int Height;

    public GameResolution CurrentResolution { get => new GameResolution(Screen.width, Screen.height); }

    public GameResolution() { }

    public GameResolution(int width, int height)
    {
        Width = width;
        Height = height;
    }
}

public static class SettingsGraphics
{
    private static Resolution[] s_resolutions;

    private static readonly FullScreenMode[] s_screenModes = new FullScreenMode[3]
        {
            FullScreenMode.FullScreenWindow,
            FullScreenMode.MaximizedWindow,
            FullScreenMode.Windowed
        };
    private static int s_currentScreenModeIndex = 0;

    public static Resolution[] Resolutions { get => s_resolutions; }

    public static GameResolution CurrentResolution { get; private set; }
    public static int CurrentScreenModeIndex { get => s_currentScreenModeIndex; }
    public static int CurrentQualityLevelIndex { get => QualitySettings.GetQualityLevel(); }
    public static bool IsShadowsEnabled { get => QualitySettings.shadows != ShadowQuality.Disable; }

    public static void Initialize()
    {
        InitializeResolutions();
    }

    #region Resolution
    public static void SetResolution(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreenMode);
        CurrentResolution = new GameResolution(width, height);
    }

    public static void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(s_resolutions[resolutionIndex].width,
            s_resolutions[resolutionIndex].height, Screen.fullScreenMode);

        CurrentResolution = new GameResolution(s_resolutions[resolutionIndex].width,
            s_resolutions[resolutionIndex].height);
    }

    private static void InitializeResolutions()
    {
        List<Resolution> resolutions = new List<Resolution>();
        Resolution[] screenResolutions = Screen.resolutions;
        for (int i = 0; i < screenResolutions.Length; i++)
        {
            if (i + 1 < screenResolutions.Length &&
                screenResolutions[i].width == screenResolutions[i + 1].width &&
                screenResolutions[i].height == screenResolutions[i + 1].height) continue;

            resolutions.Add(screenResolutions[i]);
        }
        s_resolutions = resolutions.ToArray();
    }
    #endregion

    #region ScreenMode
    public static void SetScreenMode(int screenModeIndex)
    {
        s_currentScreenModeIndex = screenModeIndex;
        Screen.fullScreenMode = s_screenModes[screenModeIndex];
    }
    #endregion

    #region QualityLevel
    public static void SetQualityLevel(int qualityLevelIndex)
    {
        QualitySettings.SetQualityLevel(qualityLevelIndex);
    }
    #endregion

    #region Shadows
    public static void SetShadowsEnabled(bool isEnabled)
    {
        if (isEnabled)
            QualitySettings.shadows = ShadowQuality.All;
        else
            QualitySettings.shadows = ShadowQuality.Disable;
    }
    #endregion
}
