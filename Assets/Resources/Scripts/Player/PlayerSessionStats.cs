using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSessionStats : NetworkBehaviour
{
    [field: SyncVar]
    public int Kills { get; set; }

    [field: SyncVar]
    public int LapsCompleted { get; set; }

    private void Start()
    {
        if ((GameMode)ServerData.GameModeIndex != GameMode.Free)
            StartCoroutine(InitializeEvents());
    }

    private IEnumerator InitializeEvents()
    {
        while (GameModeBase.Instance == null)
            yield return null;

        GameModeBase.Instance.OnGameStarted += CmdResetAllStats;
        GameModeBase.Instance.OnGameEnded += CmdResetAllStats;
    }

    [Command(requiresAuthority = false)]
    public void CmdSetKills(int kills)
    {
        Kills = kills;
    }

    [Command(requiresAuthority = false)]
    public void CmdSetLapsCompleted(int lapsCompleted)
    {
        LapsCompleted = lapsCompleted;
    }

    [Command(requiresAuthority = false)]
    public void CmdResetAllStats()
    {
        Kills = 0;
        LapsCompleted = 0;
    }
}
