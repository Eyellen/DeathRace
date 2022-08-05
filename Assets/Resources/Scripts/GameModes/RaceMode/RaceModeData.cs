using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceModeData : GameModeDataBase
{
    public int LapsToWin { get; set; }
    public int ActivateTilesOnLap { get; set; }
    public int ReactivateTilesAfterLap { get; set; }
    public int ReactivateTilesAfterSeconds { get; set; }

    public RaceModeData(
        int lapsToWin,
        int activateTilesOnLap,
        int reactivateTilesAfterLap,
        int reactivateTilesAfterSeconds
        )
    {
        LapsToWin = lapsToWin;
        ActivateTilesOnLap = activateTilesOnLap;
        ReactivateTilesAfterLap = reactivateTilesAfterLap;
        ReactivateTilesAfterSeconds = reactivateTilesAfterSeconds;
    }
}
