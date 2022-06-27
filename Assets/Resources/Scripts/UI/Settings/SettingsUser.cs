using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUser : MonoBehaviour
{
    private string _username;

    private Vector2 _sensitivity;

    [SerializeField] private GameObject _sensitivityField;
    [SerializeField] private GameObject _xSensitivityField;
    [SerializeField] private GameObject _ySensitivityField;

    public string Username { get => _username; }
    public Vector2 Sensitivity { get => _sensitivity; }

    public void SetUsername(string username)
    {
        _username = username;
    }

    public void ToggleSplitSensitivity(bool isSplit)
    {
        _sensitivityField.SetActive(!isSplit);
        _xSensitivityField.SetActive(isSplit);
        _ySensitivityField.SetActive(isSplit);

        if(!isSplit)
        {
            _sensitivity = new Vector2(_sensitivity.x, _sensitivity.x);
        }
    }

    public void SetSensitivity(float value)
    {
        _sensitivity.x = _sensitivity.y = value;
    }

    public void SetXSensitivity(float value)
    {
        _sensitivity.x = value;
    }

    public void SetYSensitivity(float value)
    {
        _sensitivity.y = value;
    }
}
