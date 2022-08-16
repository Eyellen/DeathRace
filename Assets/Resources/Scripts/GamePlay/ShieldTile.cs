using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShieldTile : TileBase
{
    protected override void OnCarExit(GameObject car)
    {
        CmdActivateProtectionSystems(car);
    }

    [Command(requiresAuthority = false)]
    private void CmdActivateProtectionSystems(GameObject car)
    {
        if (!car.TryGetComponent(out CarProtectSystems carProtectSystems)) return;

        carProtectSystems.IsActivated = true;
    }
}
