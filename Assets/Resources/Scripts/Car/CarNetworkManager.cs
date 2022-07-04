using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarNetworkManager : NetworkBehaviour
{
    [SerializeField] private Wheel[] _wheels;

    void Start()
    {
        DisableWheelsUpdateIfNotLocalPlayer();
    }

    private void DisableWheelsUpdateIfNotLocalPlayer()
    {
        if (netIdentity.hasAuthority) return;

        foreach (var wheel in _wheels)
        {
            wheel.ToggleUpdateWheels = false;
        }
    }
}
