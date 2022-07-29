using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RaceModeManager : GameModeBase
{
    [field: SerializeField] 
    public CheckPoint[] CheckPoints { get; private set; }

    // Completed laps of each player by their car's netId
    private readonly SyncDictionary<uint, int> _playersCompletedLaps = new SyncDictionary<uint, int>();

    protected override void Start()
    {
        base.Start();

        //PlayerInput.IsButtonsBlocked = true;
        InitializeCheckPoints();
        InitializePlayersCompletedLapsDictionary();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            InitializePlayersCompletedLapsDictionary();
        }
    }

    private void OnEnable()
    {
        CheckPoints[0].transform.parent.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        CheckPoints[0].transform.parent.gameObject.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
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
                CmdSetLapsCompleted(Player.LocalPlayer.Car.GetComponent<NetworkIdentity>().netId);
                Debug.Log($"player by netId {Player.LocalPlayer.Car.GetComponent<NetworkIdentity>().netId} completed lap");
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
    private void CmdSetLapsCompleted(uint netId)
    {
        _playersCompletedLaps[netId]++;
        Debug.Log($"player by netId {netId} made {_playersCompletedLaps[netId]} lap");
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
        // Looking for Players cars netIds
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
        uint[] netIds = new uint[cars.Length];
        for (int i = 0; i < netIds.Length; i++)
        {
            netIds[i] = cars[i].GetComponent<NetworkIdentity>().netId;
        }

        foreach (var netId in netIds)
        {
            // Initializing every players completed laps count by 0
            _playersCompletedLaps[netId] = 0;
        }
    }
}
