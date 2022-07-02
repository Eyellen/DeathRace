using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenu;

    private void OnEnable()
    {
        PlayerInput.IsBlocked = true;
        CursorManager.ShowCursor();
    }

    private void OnDisable()
    {
        PlayerInput.IsBlocked = false;
        CursorManager.HideCursor();

        if(_settingsMenu.activeSelf)
        {
            _settingsMenu.GetComponentInChildren<SettingsSaveUI>().SaveSettings();
            _settingsMenu.SetActive(false);
        }
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
