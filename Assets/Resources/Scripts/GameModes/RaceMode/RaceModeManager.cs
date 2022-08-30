using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RaceModeManager : GameModeBase
{
    [field: SerializeField] 
    public CheckPoint[] CheckPoints { get; private set; }

    // Completed laps of each player by their car's netId
    private readonly SyncDictionary<CarInfo, int> _playersCompletedLaps = new SyncDictionary<CarInfo, int>();

    [field: SyncVar]
    [field: SerializeField]
    public int LapsToWin { get; private set; } = 1;
    [field: SyncVar]
    public int ActivateTilesOnLap { get; private set; }
    [field: SyncVar]
    public int TilesCooldown { get; private set; }

    private bool IsTilesActivated;

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        InitializeGameModeSettings();
        InitializeAllTiles();

        InitializeCheckPoints();

        if (!IsGameOn && !IsGameStarting)
            MessageWaitingForPlayers();

        return true;
    }

    protected override void Update()
    {
        base.Update();

        // Info for debugging
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (var info in _playersCompletedLaps)
            {
                Debug.Log($"Car by netId {info.Key} have made {info.Value} laps");
            }
        }
    }

    [ServerCallback]
    protected override void ServerUpdate()
    {
        base.ServerUpdate();

        // Starting game when there is enough players
        if (!IsGameOn && !IsGameStarting && // This condition is not required here but written for optimization
            // Since if the conditions above will be false then the condition below will not be checked
            GameObject.FindGameObjectsWithTag("Car").Length == ServerData.MaxPlayersCount)
        {
            StartCoroutine(StartGameCoroutine(3));
        }
    }

    public override void Enable()
    {
        base.Enable();

        //CheckPoints[0].transform.parent.gameObject.SetActive(true);
    }

    public override void Disable()
    {
        base.Disable();

        CheckPoints[0].transform.parent.gameObject.SetActive(false);
    }

    [Server]
    protected override bool StartGame()
    {
        if (!base.StartGame()) return false;


        ResetAllTiles();
        if (ActivateTilesOnLap == 0)
            SetActiveAllTiles(true);

        SpawnManager.Instance.RespawnAllPlayers();

        // Need some latency because old cars will be destroyed only on next frame
        //InitializePlayersCompletedLapsDictionary();

        // Skipping 1 frame to wait till old cars will be destroyed and invoking InitializePlayersCompletedLapsDictionary()
        StartCoroutine(Invoke(InitializePlayersCompletedLapsDictionary, afterFrames: 1));

        // No need to call here because it's being called in base method
        //RpcStartGame();

        MessageManager.Instance.RpcShowBottomMessage("Get Ready");

        return true;
    }

    [ClientRpc]
    protected override void RpcStartGame()
    {
        base.RpcStartGame();

        CheckPoints[0].transform.parent.gameObject.SetActive(true);

        PlayerInput.IsButtonsBlocked = true;
        StartCoroutine(UnblockPlayersInputCoroutine(inSeconds: 3));
    }

    [Server]
    protected override bool StopGame()
    {
        if (!base.StopGame()) return false;

        AnnounceTheWinner();
        ResetAllTiles();

        // No need to call here because it's being called in base method
        //RpcStopGame();

        return true;
    }

    [ClientRpc]
    protected override void RpcStopGame()
    {
        base.RpcStopGame();

        CheckPoints[0].transform.parent.gameObject.SetActive(false);
        ResetAllCheckPoints();
        MessageWaitingForPlayers();
    }

    private void InitializeCheckPoints()
    {
        for (int i = 0; i < CheckPoints.Length; i++)
        {
            // Initializing index
            CheckPoints[i].CheckPointIndex = i;

            // Initializing event
            CheckPoints[i].OnCheckPointPassed += OnCheckPointPassed;
        }
    }

    private void OnCheckPointPassed(int checkPointIndex)
    {
        // When passing last point we are resetting initial point to be able to pass it
        if (checkPointIndex == CheckPoints.Length - 1)
        {
            CheckPoints[0].ResetPoint();
        }

        if (checkPointIndex == 0)
        {
            if(CheckPoints[CheckPoints.Length - 1].IsPassed)
            {
                CmdSetLapsCompleted(Player.LocalPlayer.Car.GetComponent<CarInfo>());
                ResetAllCheckPoints();
            }

            CheckPoints[checkPointIndex].MarkAsPassed();

            return;
        }

        // Mark current point as passed only if previous point was passed
        if (CheckPoints[checkPointIndex - 1].IsPassed)
        {
            CheckPoints[checkPointIndex].MarkAsPassed();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetLapsCompleted(CarInfo carInfo)
    {
        _playersCompletedLaps[carInfo]++;
        carInfo.Player.SessionStats.CmdSetLapsCompleted(_playersCompletedLaps[carInfo]);

        if (!IsTilesActivated &&
            _playersCompletedLaps[carInfo] == ActivateTilesOnLap)
            SetActiveAllTiles(true);

        if (_playersCompletedLaps[carInfo] >= LapsToWin)
            StopGame();
    }

    private void ResetAllCheckPoints()
    {
        foreach (var checkPoint in CheckPoints)
        {
            checkPoint.ResetPoint();
        }
    }

    [ServerCallback]
    private void InitializePlayersCompletedLapsDictionary()
    {
        _playersCompletedLaps.Clear();
        
#if UNITY_EDITOR
        Debug.Log("Initializing players completed laps dictionary");
#endif
        CarInfo[] carInfos = FindObjectsOfType<CarInfo>();
        foreach (var carInfo in carInfos)
        {
            // Initializing every players completed laps count by 0
            _playersCompletedLaps[carInfo] = 0;
#if UNITY_EDITOR
            Debug.Log($"Car NetId: {netId} | Laps: {_playersCompletedLaps[carInfo]}");
#endif
        }
    }

    private IEnumerator Invoke(System.Action method, int afterFrames)
    {
        while(afterFrames > 0)
        {
            yield return null;
            afterFrames--;
        }
        method();
    }

    private void AnnounceTheWinner()
    {
        if(SpawnManager.Instance.SpawnedCars.Count == 0)
        {
            MessageManager.Instance.RpcShowTopMessage("Round Draw");
            return;
        }

        if(SpawnManager.Instance.SpawnedCars.Count == 1)
        {
            foreach (var car in SpawnManager.Instance.SpawnedCars.Values)
            {
                MessageManager.Instance.RpcShowTopMessage($"{car.GetComponent<CarInfo>().Player.Username} Won The Game");
                return;
            }
        }

        CarInfo winnerCarInfo = null;
        int maxScore = 0;
        foreach (var playerScore in _playersCompletedLaps)
        {
            if (playerScore.Value <= maxScore) continue;

            maxScore = playerScore.Value;
            winnerCarInfo = playerScore.Key;
        }

        if (winnerCarInfo == null)
        {
            MessageManager.Instance.RpcShowTopMessage("Round Draw");
        }
        else
        {
            MessageManager.Instance.RpcShowTopMessage($"{winnerCarInfo.Player.Username} Won The Game");
        }
    }

    private void MessageWaitingForPlayers()
    {
        MessageManager.Instance.ShowBottomMessage("Waiting for other players to start." + (isServer ? "\nPress P to start now." : string.Empty));
    }

    [ServerCallback]
    private void InitializeGameModeSettings()
    {
        var data = ServerData.CurrentGameModeData as RaceModeData;
        LapsToWin = data.LapsToWin;
        ActivateTilesOnLap = data.ActivateTilesOnLap;
        TilesCooldown = data.TilesCooldown;
    }

    [Server]
    private void InitializeAllTiles()
    {
        TileBase[] tiles = FindObjectsOfType<TileBase>();
        foreach (var tile in tiles)
        {
            tile.Cooldown = TilesCooldown;

            if (ActivateTilesOnLap > 0)
                tile.SetReady(false);
            else
                tile.SetReady(true);
        }
    }

    [Server]
    private void SetActiveAllTiles(bool isActive)
    {
        TileBase[] tiles = FindObjectsOfType<TileBase>();
        foreach (var tile in tiles)
        {
            tile.SetReady(isActive);
        }
        IsTilesActivated = isActive;

        if (isActive)
            MessageManager.Instance.RpcShowTopMessage("All Tiles Activated", 3);
    }

    [Server]
    private void ResetAllTiles()
    {
        TileBase[] tiles = FindObjectsOfType<TileBase>();
        foreach (var tile in tiles)
        {
            tile.ResetTile();
        }
        IsTilesActivated = false;
    }

    private IEnumerator UnblockPlayersInputCoroutine(float inSeconds)
    {
        yield return new WaitForSeconds(inSeconds);

        PlayerInput.IsButtonsBlocked = false;
        MessageManager.Instance.ShowBottomMessage("Start!", 2);
    }
}
