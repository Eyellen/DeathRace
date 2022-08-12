using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceModeData : GameModeDataBase
{
    public int LapsToWin { get; set; }
    public int ActivateTilesOnLap { get; set; }
    public int TilesCooldown { get; set; }

    public RaceModeData(
        int lapsToWin,
        int activateTilesOnLap,
        int tilesCooldown
        )
    {
        LapsToWin = lapsToWin;
        ActivateTilesOnLap = activateTilesOnLap;
        TilesCooldown = tilesCooldown;
    }
}
