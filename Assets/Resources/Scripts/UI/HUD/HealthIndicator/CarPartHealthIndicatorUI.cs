using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPartHealthIndicatorUI : HealthIndicatorBaseUI
{
    protected override IEnumerator AsignDamageableCoroutine()
    {
        while (Player.LocalPlayer.Car == null)
            yield return null;

        _damageable = Player.LocalPlayer.Car.GetComponent<CarBackPlateDamageable>();
    }
}
