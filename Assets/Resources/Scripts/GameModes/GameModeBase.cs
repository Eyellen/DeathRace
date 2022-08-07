using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameModeBase : NetworkBehaviour
{
#if UNITY_EDITOR
    [SerializeField] protected bool _isDebugging = false;
#endif

    public static GameModeBase Instance { get; private set; }

    private bool _isInitialized = false;

    [field: SyncVar]
    public bool IsGameStarting { get; private set; }

    [field: SyncVar]
    public bool IsGameOn { get; private set; }

    public static Action OnInitialized;

    public Action OnGameStarted;

    public Action OnGameEnded;

    private void InitializeInstance()
    {
        if (Instance == null)
            Instance = this;
        else
        {
#if UNITY_EDITOR
            if (_isDebugging)
                Debug.LogWarning($"Trying to create a duplicate of {nameof(GameModeBase)} when it's not allowed. " +
                    $"The duplicate will be destroyed");
#endif
            Destroy(gameObject);
        }
    }

    public virtual bool Initialize()
    {
        if (_isInitialized) return false;
        _isInitialized = true;

        InitializeInstance();

#if UNITY_EDITOR
        if (_isDebugging)
        {
            OnInitialized += () => Debug.Log("OnInitialized called");
            OnGameStarted += () => Debug.Log("OnGameStarted called");
            OnGameEnded += () => Debug.Log("OnGameEnded called");
        }
#endif
        OnInitialized?.Invoke();

        return true;
    }

    public virtual void Enable() 
    {
        Initialize();
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

    private void OnDestroy()
    {
        // Clears all subscriptions from OnInitialized static event
        // because it can cause errors
        OnInitialized = null;
    }

    [ServerCallback]
    protected virtual void ServerUpdate()
    {
        // Starting game by button
        if (Input.GetKeyDown(KeyCode.P) && SpawnManager.Instance.SpawnedCars.Count >= 2)
        {
            StartCoroutine(StartGameCoroutine(3));
        }

        // Start game if Host is spectating and there is at least 2 not spectating players
        if (Player.LocalPlayer.SelectedCarIndex == -1 &&
            SpawnManager.Instance.SpawnedCars.Count >= 2)
        {
            StartCoroutine(StartGameCoroutine(3));
        }

        // If there is 1 or less players then game ends
        if (SpawnManager.Instance.SpawnedCars.Count <= 1)
        {
            StopGame();
        }
    }

    [Server]
    protected virtual bool StartGame() 
    {
        if (IsGameOn) return false;
        IsGameStarting = false;
        IsGameOn = true;
        RpcStartGame();
        MessageManager.Instance.RpcHideAllMessages();
        GameManager.Instance.ClearScene();
        Debug.Log("StartGame called");
        return true;
    }

    [ClientRpc]
    protected virtual void RpcStartGame()
    {
        IsGameStarting = false;
        IsGameOn = true;
        Debug.Log("RpcStartGame called");
        OnGameStarted?.Invoke();
    }

    [Server]
    protected virtual bool StopGame() 
    {
        if (!IsGameOn) return false;
        IsGameOn = false;
        RpcStopGame();

        return true;
    }

    [ClientRpc]
    protected virtual void RpcStopGame()
    {
        IsGameOn = false;

        OnGameEnded?.Invoke();
    }

    [Server]
    public void RestartGame()
    {
        StopGame();

        if (SpawnManager.Instance.SpawnedCars.Count >= 2)
            StartCoroutine(StartGameCoroutine(3));
    }

    /// <summary>
    /// Server method.
    /// Starts game in seconds via StartGame() method and displays message to all clients
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    [Server]
    protected IEnumerator StartGameCoroutine(int seconds)
    {
        if (IsGameOn || IsGameStarting) yield break;
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

    //protected virtual bool CheckIfCanStartGame()
    //{
    //    // Starting game by button
    //    if (Input.GetKeyDown(KeyCode.P) && SpawnManager.Instance.SpawnedCars.Count >= 2)
    //    {
    //        return true;
    //    }

    //    // Start game if Host is spectating and there is at least 2 not spectating players
    //    if (Player.LocalPlayer.SelectedCarIndex == -1 &&
    //        SpawnManager.Instance.SpawnedCars.Count >= 2)
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}
