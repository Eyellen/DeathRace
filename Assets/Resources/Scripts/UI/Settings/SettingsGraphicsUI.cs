using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsGraphicsUI : MonoBehaviour
{
    [Header("Resolutions category")]
    [SerializeField] private TMP_Dropdown _resolutionsDropdown;
    
    [Header("Display Mode category")]
    [SerializeField] private TMP_Dropdown _displayModeDropdown;

    [Header("Quality category")]
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    [Header("Shadows category")]
    [SerializeField] private Toggle _shadowsToggle;

    private void Start()
    {
        InitializeResolutions();
        RefreshOptions();
    }

    #region Resolution
    public void SetResolution(int resolutionIndex)
    {
        SettingsGraphics.SetResolution(resolutionIndex);
        RefreshOptions();
    }

    private void InitializeResolutions()
    {
        List<string> options = new List<string>();
        Resolution[] resolutions = SettingsGraphics.Resolutions;
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (SettingsGraphics.CurrentResolution != null)
            {
                if (resolutions[i].width == SettingsGraphics.CurrentResolution.Width &&
                    resolutions[i].height == SettingsGraphics.CurrentResolution.Height)
                    currentResolutionIndex = i;
            }
            else
            {
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                    currentResolutionIndex = i;
            }

            options.Add($"{resolutions[i].width}x{resolutions[i].height}");
        }

        _resolutionsDropdown.ClearOptions();
        _resolutionsDropdown.AddOptions(options);

        _resolutionsDropdown.value = currentResolutionIndex;
        _resolutionsDropdown.RefreshShownValue();
    }

    private int CurrentResolutionIndex()
    {
        Resolution[] resolutions = SettingsGraphics.Resolutions;
        int currentResolutionIndex = 0;
        foreach (var resolution in resolutions)
        {
            if (resolution.width == Screen.currentResolution.width &&
                resolution.height == Screen.currentResolution.height)
                return currentResolutionIndex;

            currentResolutionIndex++;
        }
        return currentResolutionIndex;
    }
    #endregion

    #region DisplayMode
    public void SetScreenMode(int screenModeIndex)
    {
        SettingsGraphics.SetScreenMode(screenModeIndex);
        RefreshOptions();
    }
    #endregion

    #region Quality
    public void SetQualityLevel(int qualityLevelIndex)
    {
        SettingsGraphics.SetQualityLevel(qualityLevelIndex);
        RefreshOptions();
    }
    #endregion

    #region Shadows
    public void SetShadows(bool isShadowsEnabled)
    {
        SettingsGraphics.SetShadowsEnabled(isShadowsEnabled);
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
        _resolutionsDropdown.RefreshShownValue();
        _displayModeDropdown.value = SettingsGraphics.CurrentScreenModeIndex;
        _qualityDropdown.value = SettingsGraphics.CurrentQualityLevelIndex;
        _shadowsToggle.isOn = SettingsGraphics.IsShadowsEnabled;
    }
}
