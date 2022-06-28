using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUser : MonoBehaviour
{
    [Header("Username settings")]
    private string _username;
    [SerializeField] private TMP_InputField _usernameField;

    [Header("Sensitivity settings")]
    private bool _isSensitivitySplitted;
    [SerializeField] private Toggle _splitSensitivityToggle;
    private Vector2 _sensitivity;
    [SerializeField] private GameObject _sensitivityField;
    [SerializeField] private GameObject _xSensitivityField;
    [SerializeField] private GameObject _ySensitivityField;

    public string Username { get => _username; }
    public bool IsSensitivitySplitted { get => _isSensitivitySplitted; }
    public Vector2 Sensitivity { get => _sensitivity; }

    public void SetUsername(string username)
    {
        _username = username;
        RefreshOptions();
    }

    public void ToggleSplitSensitivity(bool isSplit)
    {
        _isSensitivitySplitted = isSplit;

        _sensitivityField.SetActive(!isSplit);
        _xSensitivityField.SetActive(isSplit);
        _ySensitivityField.SetActive(isSplit);

        if(!isSplit)
        {
            _sensitivity = new Vector2(_sensitivity.x, _sensitivity.x);
        }
        RefreshOptions();
    }

    public void SetSensitivity(float value)
    {
        _sensitivity.x = _sensitivity.y = value;
        RefreshOptions();
    }

    public void SetXSensitivity(float value)
    {
        _sensitivity.x = value;
        RefreshOptions();
    }

    public void SetYSensitivity(float value)
    {
        _sensitivity.y = value;
        RefreshOptions();
    }

    private void RefreshOptions()
    {
        _usernameField.text = _username;

        _splitSensitivityToggle.isOn = _isSensitivitySplitted;

        if(_isSensitivitySplitted)
        {
            _xSensitivityField.GetComponentInChildren<Slider>().value = _sensitivity.x;
            _ySensitivityField.GetComponentInChildren<Slider>().value = _sensitivity.y;
        }
        else
        {
            _sensitivityField.GetComponentInChildren<Slider>().value = _sensitivity.x;
        }
    }
}
