using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsGraphics : MonoBehaviour
{
    [Header("Resolutions category")]
    private Resolution[] _screenResolutions;
    [SerializeField] private TMP_Dropdown _resolutionsDropdown;

    [Header("Display Mode category")]
    private FullScreenMode[] _screenModes;
    [SerializeField] private TMP_Dropdown _displayModeDropdown;

    private void Start()
    {
        //Debug.Log(nameof(FullScreenMode.ExclusiveFullScreen));

        InitializeResolutions();
        InitializeDisplayMode();
    }



    #region Resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _screenResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    private void InitializeResolutions()
    {
        _screenResolutions = Screen.resolutions;

        _resolutionsDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < _screenResolutions.Length; i++)
        {
            string option = $"{_screenResolutions[i].width}x{_screenResolutions[i].height}";
            options.Add(option);

            if (_screenResolutions[i].width == Screen.currentResolution.width &&
                _screenResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionsDropdown.AddOptions(options);

        _resolutionsDropdown.value = currentResolutionIndex;
        _resolutionsDropdown.RefreshShownValue();
    }
    #endregion



    #region DisplayMode
    public void SetDisplayMode(int displayModeIndex)
    {
        Screen.fullScreenMode = _screenModes[displayModeIndex];
    }

    private void InitializeDisplayMode()
    {
        _displayModeDropdown.ClearOptions();

        _screenModes = new FullScreenMode[]
        {
            FullScreenMode.ExclusiveFullScreen,
            FullScreenMode.MaximizedWindow,
            FullScreenMode.Windowed
        };

        List<string> options = new List<string>()
        {
            "Exclusive FullScreen",
            "Maximized Window",
            "Windowed"
        };

        _displayModeDropdown.AddOptions(options);
    }
    #endregion



    #region Quality
    public void SetQualityLevel(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    #endregion



    #region Shadows
    public void SetShadows(bool isShadowsEnabled)
    {
        if (isShadowsEnabled)
            QualitySettings.shadows = ShadowQuality.All;
        else
            QualitySettings.shadows = ShadowQuality.Disable;
    }
    #endregion



    //#region RenderDistance
    //public void SetRenderDistance(float value)
    //{
        
    //}
    //#endregion
}
