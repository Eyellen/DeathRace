using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaceModeSettingsUI : GameModeSettingsBaseUI
{
    [SerializeField] private Slider _lapsToWinSlider;
    [SerializeField] private TextMeshProUGUI _lapsToWinText;

    [SerializeField] private Slider _activateTilesOnLapSlider;
    [SerializeField] private TextMeshProUGUI _activateTilesOnLapText;

    [SerializeField] private TMP_Dropdown _tilesCooldownDropdown;

    private RaceModeData _raceModeData;

    private void OnEnable()
    {
        InitializeSettings();
    }

    public void SetLapsToWin(float laps)
    {
        _lapsToWinText.text = laps.ToString();
        _raceModeData.LapsToWin = (int)laps;
        _activateTilesOnLapSlider.maxValue = (int)laps - 1;
    }

    public void SetActivateTilesOnLap(float lapNumber)
    {
        _activateTilesOnLapText.text = lapNumber.ToString();
        _raceModeData.ActivateTilesOnLap = (int)lapNumber;
    }

    public void SetTilesCooldown(int index)
    {
        switch (index)
        {
            case 0:
                // 5 seconds
                _raceModeData.TilesCooldown = 5;
                break;

            case 1:
                // 15 seconds
                _raceModeData.TilesCooldown = 15;
                break;

            case 2:
                // 30 seconds
                _raceModeData.TilesCooldown = 30;
                break;

            case 3:
                // 1 minute
                _raceModeData.TilesCooldown = 60;
                break;

            case 4:
                // 3 minutes
                _raceModeData.TilesCooldown = 180;
                break;

            case 5:
                // 5 minutes
                _raceModeData.TilesCooldown = 300;
                break;

            default:
                break;
        }
    }

    private void InitializeSettings()
    {
        _raceModeData = new RaceModeData(5, 3, 3);
        ServerData.CurrentGameModeData = _raceModeData;
        _lapsToWinSlider.value = 5;
        _activateTilesOnLapSlider.value = 3;
        _tilesCooldownDropdown.value = 3;
    }
}
