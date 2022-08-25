using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSessionStats : NetworkBehaviour
{
    [SyncVar]
    private int _playerStatusIndex = 0;
    public PlayerStatus PlayerStatus { get => (PlayerStatus)_playerStatusIndex; set => _playerStatusIndex = (int)value; }

    [field: SyncVar]
    public int Kills { get; set; }

    [field: SyncVar]
    public int LapsCompleted { get; set; }

    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();

        if ((GameMode)ServerData.GameModeIndex != GameMode.Free)
            StartCoroutine(InitializeEvents());

        if (isServer)
            StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator InitializeEvents()
    {
        while (GameModeBase.Instance == null)
            yield return null;

        GameModeBase.Instance.OnGameStarted += CmdResetAllStats;
        GameModeBase.Instance.OnGameEnded += CmdResetAllStats;
    }

    [ServerCallback]
    private IEnumerator UpdateCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.2f);

            if(_player.SelectedCarIndex == -1)
            {
                PlayerStatus = PlayerStatus.Spectator;
            }
            else if(_player.Car == null)
            {
                PlayerStatus = PlayerStatus.Eliminated;
            }
            else
            {
                PlayerStatus = PlayerStatus.Alive;
            }

            yield return null;
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSetPlayerStatus(PlayerStatus status)
    {
        PlayerStatus = status;
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
