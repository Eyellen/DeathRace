using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectUI : MonoBehaviour
{
    [SerializeField] private Button _spawnButton;

    private void Start()
    {
        if (GameModeBase.Instance != null)
            GameModeBase.OnInitialized += InitializeEvents;
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
        //if (SpawnManager.Instance.SelectedCar == null) return;
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
        Player.LocalPlayer.Car?.GetComponent<CarDamageable>().DestroySelf();
    }

    public void CheckIfSpawnIsAllowed()
    {
        if (GameModeBase.Instance != null)
            _spawnButton.interactable = !GameModeBase.Instance.IsGameOn;
    }

    private void InitializeEvents()
    {
        CheckIfSpawnIsAllowed();
        GameModeBase.Instance.OnGameStarted += CheckIfSpawnIsAllowed;
        GameModeBase.Instance.OnGameEnded += CheckIfSpawnIsAllowed;
    }
}
