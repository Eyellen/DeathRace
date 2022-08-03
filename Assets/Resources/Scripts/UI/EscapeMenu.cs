using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenu;

    [SerializeField] private Button _restartButton;

    private void Start()
    {
        if ((GameMode)ServerData.GameModeIndex != GameMode.Free)
            GameModeBase.OnInitialized += InitializeRestartButton;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        PlayerInput.IsBlocked = true;
        CursorManager.ShowCursor();
    }

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

    private void InitializeRestartButton()
    {
        if(NetworkServer.active)
        {
            _restartButton.onClick.AddListener(() => GameModeBase.Instance.RestartGame());
            GameModeBase.Instance.OnGameStarted += () => _restartButton.interactable = true;
            GameModeBase.Instance.OnGameEnded += () => _restartButton.interactable = false;
        }
    }
}
