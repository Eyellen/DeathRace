using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTile : TileBase
{
    protected override void OnCarEnter(GameObject car)
    {
        if(car.TryGetComponent(out GunBase gunBase))
        {
            GunBase[] guns = car.GetComponents<GunBase>();
            foreach (var gun in guns)
                gun.enabled = true;
        }
        
        if(car.TryGetComponent(out RocketLauncher rocketLauncher))
        {
            rocketLauncher.enabled = true;
        }
    }}
