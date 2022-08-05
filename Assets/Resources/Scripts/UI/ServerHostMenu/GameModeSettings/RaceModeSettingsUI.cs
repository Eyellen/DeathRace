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

    [SerializeField] private Slider _reactivateTilesAfterLapSlider;
    [SerializeField] private TextMeshProUGUI _resetTilesAfterLapText;

    [SerializeField] private Slider _reactivateTilesAfterSecondsSlider;
    [SerializeField] private TextMeshProUGUI _resetTilesAfterSecondsText;

    private RaceModeData _raceModeData = new RaceModeData(5, 3, 1, 15);

    private void OnEnable()
    {
        InitializeSettings();
    }

    public void SetLapsToWin(float laps)
    {
        _lapsToWinText.text = laps.ToString();
        _raceModeData.LapsToWin = (int)laps;
    }

    public void SetActivateTilesOnLap(float lapNumber)
    {
        _activateTilesOnLapText.text = lapNumber.ToString();
        _raceModeData.ActivateTilesOnLap = (int)lapNumber;
    }

    public void SetReactivateTilesAfterLap(float lapNumber)
    {
        _resetTilesAfterLapText.text = lapNumber.ToString();
        _raceModeData.ReactivateTilesAfterLap = (int)lapNumber;
    }

    public void SetReactivateTilesAfterSeconds(float seconds)
    {
        _resetTilesAfterSecondsText.text = seconds.ToString() + "s";
        _raceModeData.ReactivateTilesAfterSeconds = (int)seconds;
    }

    private void InitializeSettings()
    {
        ServerData.CurrentGameModeData = _raceModeData;
        _lapsToWinSlider.value = _raceModeData.LapsToWin;
        _activateTilesOnLapSlider.value = _raceModeData.ActivateTilesOnLap;
        _reactivateTilesAfterLapSlider.value = _raceModeData.ReactivateTilesAfterLap;
        _reactivateTilesAfterSecondsSlider.value = _raceModeData.ReactivateTilesAfterSeconds;
    }
}
