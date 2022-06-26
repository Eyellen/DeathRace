using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUser : MonoBehaviour
{
    private string _username;

    [SerializeField] private GameObject _sensitivityField;
    [SerializeField] private GameObject _xSensitivityField;
    [SerializeField] private GameObject _ySensitivityField;

    public void SetUsername(string username)
    {
        _username = username;
    }

    public void ToggleSplitSensitivity(bool isSplit)
    {
        _sensitivityField.SetActive(!isSplit);
        _xSensitivityField.SetActive(isSplit);
        _ySensitivityField.SetActive(isSplit);
    }
}
