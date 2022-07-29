using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[DisallowMultipleComponent]
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SyncVar]
    public GameMode CurrentGameMode { get; private set; }

    [SerializeField] private RaceModeManager _raceModeManager;

    private void Start()
    {
        InitializeInstance();
        InitializeGameModeManager();
    }

    private void InitializeInstance()
    {
        if (Instance == null)
            Instance = this;
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Trying to create a duplicate of {nameof(GameManager)} when it's not allowed. " +
                $"The duplicate will be destroyed");
#endif
            Destroy(gameObject);
        }
    }

    [ServerCallback]
    private void InitializeGameModeManager()
    {
        CurrentGameMode = (GameMode)ServerData.GameModeIndex;

        switch (CurrentGameMode)
        {
            case GameMode.Free:
                {
                    // FREE MODE
                    // Do nothing
                    break;
                }
            case GameMode.Race:
                {
                    // RACE MODE
                    _raceModeManager.SetActive(true);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
