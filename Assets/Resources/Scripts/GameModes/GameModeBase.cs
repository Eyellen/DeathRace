using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameModeBase : NetworkBehaviour
{
    public static GameModeBase Instance { get; private set; }

    private bool _isInitialized = false;

    [field: SyncVar]
    public bool IsWaitingForPlayers { get; protected set; }

    [field: SyncVar]
    public bool IsGameStarted { get; protected set; }

    private void InitializeInstance()
    {
        if (Instance == null)
            Instance = this;
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"Trying to create a duplicate of {nameof(GameModeBase)} when it's not allowed. " +
                $"The duplicate will be destroyed");
#endif
            Destroy(gameObject);
        }
    }

    public virtual void Initialize()
    {
        _isInitialized = true;

        InitializeInstance();
    }

    public virtual void Enable() 
    {
        if (!_isInitialized) Initialize();

        gameObject.SetActive(true);
    }

    public virtual void Disable() 
    {
        gameObject.SetActive(false);
    }

    protected virtual void StartGame() { }
    protected virtual void StopGame() { }
    protected virtual void RestartGame() { }
}
