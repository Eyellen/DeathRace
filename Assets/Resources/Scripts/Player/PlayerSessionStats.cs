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

    public void CmdSetKills(int kills)
    {
        Kills = kills;
    }

    public void CmdSetLapsCompleted(int lapsCompleted)
    {
        LapsCompleted = lapsCompleted;
    }
}
