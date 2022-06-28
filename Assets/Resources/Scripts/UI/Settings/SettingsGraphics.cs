using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsGraphics : MonoBehaviour
{
    [Header("Resolutions category")]
    [SerializeField] private TMP_Dropdown _resolutionsDropdown;
    private Resolution[] _screenResolutions;
    
    [Header("Display Mode category")]
    [SerializeField] private TMP_Dropdown _displayModeDropdown;
    private FullScreenMode[] _screenModes;

    [Header("Quality category")]
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    [Header("Shadows category")]
    [SerializeField] private Toggle _shadowsToggle;

    public Resolution CurrentResolution { get { return Screen.currentResolution; } }
    public int ResolutionIndex { get; private set; }
    public int DisplayModeIndex { get; private set; }
    public int QualityIndex { get; private set; }
    public bool IsShadowsEnabled { get; private set; }

    private void Start()
    {
        InitializeResolutions();
        InitializeDisplayMode();

        SettingsGeneralData data = SettingsSaveSystem.CachedSave;

        //Screen.SetResolution(data.graphicsData.ResolutionWidth, data.graphicsData.ResolutionHeight, Screen.fullScreenMode);
        ResolutionIndex = data.graphicsData.ResolutionIndex;
        DisplayModeIndex = data.graphicsData.DisplayModeIndex;
        QualityIndex = data.graphicsData.QualityIndex;
        IsShadowsEnabled = data.graphicsData.IsShadowsEnabled;
        RefreshOptions();
    }

    #region Resolution
    public void SetResolution(int resolutionIndex)
    {
        ResolutionIndex = resolutionIndex;
        Resolution resolution = _screenResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        RefreshOptions();
    }

    private void InitializeResolutions()
    {
        _screenResolutions = Screen.resolutions;

        _resolutionsDropdown.ClearOptions();

        List<string> options = new List<string>();
        for (int i = 0; i < _screenResolutions.Length; i++)
        {
            string option = $"{_screenResolutions[i].width}x{_screenResolutions[i].height}";
            options.Add(option);

            if (_screenResolutions[i].width == Screen.currentResolution.width &&
                _screenResolutions[i].height == Screen.currentResolution.height)
            {
                ResolutionIndex = i;
            }
        }

        _resolutionsDropdown.AddOptions(options);

        _resolutionsDropdown.value = ResolutionIndex;
        _resolutionsDropdown.RefreshShownValue();
    }
    #endregion

    #region DisplayMode
    public void SetDisplayMode(int displayModeIndex)
    {
        DisplayModeIndex = displayModeIndex;
        Screen.fullScreenMode = _screenModes[displayModeIndex];
        RefreshOptions();
    }

    private void InitializeDisplayMode()
    {
        _displayModeDropdown.ClearOptions();

        _screenModes = new FullScreenMode[]
        {
            FullScreenMode.FullScreenWindow,
            FullScreenMode.MaximizedWindow,
            FullScreenMode.Windowed
        };

        List<string> options = new List<string>()
        {
            "FullScreen Window",
            "Maximized Window",
            "Windowed"
        };

        _displayModeDropdown.AddOptions(options);
    }
    #endregion

    #region Quality
    public void SetQualityLevel(int qualityIndex)
    {
        QualityIndex = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
        RefreshOptions();
    }
    #endregion

    #region Shadows
    public void SetShadows(bool isShadowsEnabled)
    {
        IsShadowsEnabled = isShadowsEnabled;
        if (isShadowsEnabled)
            QualitySettings.shadows = ShadowQuality.All;
        else
            QualitySettings.shadows = ShadowQuality.Disable;
        RefreshOptions();
    }
    #endregion

    //#region RenderDistance
    //public void SetRenderDistance(float value)
    //{
        
    //}
    //#endregion

    private void RefreshOptions()
    {
        _resolutionsDropdown.value = ResolutionIndex;
        _displayModeDropdown.value = DisplayModeIndex;
        _qualityDropdown.value = QualityIndex;
        _shadowsToggle.isOn = IsShadowsEnabled;
    }
}
