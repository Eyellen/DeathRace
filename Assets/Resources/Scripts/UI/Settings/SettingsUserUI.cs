using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUserUI : MonoBehaviour
{
    [Header("Username settings")]
    [SerializeField] private TMP_InputField _usernameField;

    [Header("Sensitivity settings")]
    [SerializeField] private Toggle _splitSensitivityToggle;
    [SerializeField] private GameObject _sensitivityField;
    [SerializeField] private GameObject _xSensitivityField;
    [SerializeField] private GameObject _ySensitivityField;

    private Slider _sensitivitySlider;
    private Slider _xSensitivitySlider;
    private Slider _ySensitivitySlider;

    private void Awake()
    {
        _sensitivitySlider = _sensitivityField.GetComponentInChildren<Slider>();
        _xSensitivitySlider = _xSensitivityField.GetComponentInChildren<Slider>();
        _ySensitivitySlider = _ySensitivityField.GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        RefreshOptions();
    }

    public void SetUsername(string username)
    {
        if(!UsernameRequirements.CheckIfNameIsAppropriate(username))
        {
            string defaultName = "Driver";
            _usernameField.text = defaultName;
            return;
        }
        SettingsUser.SetUsername(username);
        RefreshOptions();
    }

    public void ToggleSplitSensitivity(bool isSplit)
    {
        SettingsUser.SetSensitivitySplit(isSplit);
        RefreshOptions();
    }

    public void SetSensitivity(float value)
    {
        SettingsUser.SetSensitivity(value);
        RefreshOptions();
    }

    public void SetXSensitivity(float value)
    {
        SettingsUser.SetXSensitivity(value);
        RefreshOptions();
    }

    public void SetYSensitivity(float value)
    {
        SettingsUser.SetYSensitivity(value);
        RefreshOptions();
    }

    private void RefreshOptions()
    {
        _usernameField.text = SettingsUser.Username;

        _splitSensitivityToggle.isOn = SettingsUser.IsSensitivitySplit;

        _sensitivityField.SetActive(!SettingsUser.IsSensitivitySplit);
        _xSensitivityField.SetActive(SettingsUser.IsSensitivitySplit);
        _ySensitivityField.SetActive(SettingsUser.IsSensitivitySplit);

        if (SettingsUser.IsSensitivitySplit)
        {
            _xSensitivitySlider.value = SettingsUser.Sensitivity.x;
            _ySensitivitySlider.value = SettingsUser.Sensitivity.y;
        }
        else
        {
            _sensitivitySlider.value = SettingsUser.Sensitivity.x;
        }
    }
}
