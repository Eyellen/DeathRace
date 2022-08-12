using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectUI : MonoBehaviour
{
    [SerializeField] private Button _spawnButton;

    private void Start()
    {
        if ((GameMode)ServerData.GameModeIndex != GameMode.Free)
            GameModeBase.OnInitialized += InitializeEvents;
    }

    private void Update()
    {
        CursorManager.ShowCursor();
        PlayerInput.IsBlocked = true;
    }

    private void OnEnable()
    {
        CursorManager.ShowCursor();
        PlayerInput.IsBlocked = true;
    }

    private void OnDisable()
    {
        CursorManager.HideCursor();
        PlayerInput.IsBlocked = false;
    }

    private void OnDestroy()
    {
        //GameModeBase.OnInitialized -= InitializeEvents;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Spawn()
    {
        GameCanvas.Instance.SetActiveHUD(true);

        if (Player.LocalPlayer.Car != null)
        {
            SpawnManager.Instance.RemoveCurrentCar();
        }

        SpawnManager.Instance.SpawnLocalPlayer();
        SetActive(false);
    }

    public void Spectate()
    {
        Player.LocalPlayer.CmdSetSelectedCarIndex(-1);

        if (Player.LocalPlayer.Car != null)
            Player.LocalPlayer.Car.GetComponent<CarDamageable>().DestroySelf();
    }

    public void CheckIfSpawnIsAllowed()
    {
        if ((GameMode)ServerData.GameModeIndex != GameMode.Free)
            _spawnButton.interactable = !GameModeBase.Instance.IsGameOn;
    }

    private void InitializeEvents()
    {
        CheckIfSpawnIsAllowed();
        GameModeBase.Instance.OnGameStarted += CheckIfSpawnIsAllowed;
        GameModeBase.Instance.OnGameEnded += CheckIfSpawnIsAllowed;
    }
}
