using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameModeBase : NetworkBehaviour
{
    public static GameModeBase Instance { get; private set; }

    [field: SyncVar]
    public bool IsWaitingForPlayers { get; protected set; }

    [field: SyncVar]
    public bool IsGameStarted { get; protected set; }

    protected virtual void Start()
    {
        InitializeInstance();
    }

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
}
