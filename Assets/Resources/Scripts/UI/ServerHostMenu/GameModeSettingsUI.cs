using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSettingsUI : MonoBehaviour
{
    [SerializeField] private GameModeSettingsBaseUI _raceModeSettingsUI;

    private GameModeSettingsBaseUI _currentSettings;

    public void SetGameModeSettings(int gameModeIndex)
    {
        _currentSettings?.SetActive(false);
        switch ((GameMode)gameModeIndex)
        {
            case GameMode.Free:
                _currentSettings = null;
                return;

            case GameMode.Race:
                _currentSettings = _raceModeSettingsUI;
                break;

            default:
                break;
        }
        _currentSettings.SetActive(true);
    }
}
