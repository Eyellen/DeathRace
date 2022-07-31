using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameModeBase : NetworkBehaviour
{
    public static GameModeBase Instance { get; private set; }

    private bool _isInitialized = false;

    [field: SyncVar]
    public bool IsWaitingForPlayers { get; private set; }

    [field: SyncVar]
    public bool IsGameStarting { get; private set; }

    [field: SyncVar]
    public bool IsGameOn { get; private set; }

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

    protected virtual void Update()
    {
        ServerUpdate();
    }
    
    [ServerCallback]
    protected virtual void ServerUpdate()
    {
        // Starting game by button
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(StartGameCoroutine(3));
        }
    }

    [Server]
    protected virtual void StartGame() 
    {
        if (IsGameOn) return;
        IsGameStarting = false;
        IsGameOn = true;
    }

    [Server]
    protected virtual void StopGame() { }

    [Server]
    protected virtual void RestartGame() { }

    /// <summary>
    /// Server method.
    /// Starts game in seconds via StartGame() method and displays message to all clients
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    [Server]
    protected IEnumerator StartGameCoroutine(int seconds)
    {
        if (IsGameStarting) yield break;
        IsGameStarting = true;

        while (seconds > 0)
        {
            MessageManager.Instance.RpcShowBottomMessage($"The game is starting in {seconds} seconds");
            yield return new WaitForSeconds(1);
            seconds--;
        }
        MessageManager.Instance.RpcHideBottomMessage();

        StartGame();
    }
}
