using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarLights : NetworkBehaviour
{
    [SyncVar]
    private LightMode _currentLightMode = LightMode.None;
    private LightMode _previousLightMode;

    [Header("Near Lights")]
    [SerializeField]
    private Light[] _nearLights;

    [Header("Far Lights")]
    [SerializeField]
    private Light[] _farLights;

    private void Update()
    {
        if(PlayerInput.IsLightsPressed)
        {
            _currentLightMode = (LightMode)((int)(_currentLightMode + 1) % Enum.GetValues(typeof(LightMode)).Length);
        }

        if(_currentLightMode != _previousLightMode)
        {
            _previousLightMode = _currentLightMode;
            ToggleLights(_currentLightMode);
        }
    }

    private void ToggleLights(LightMode lightMode)
    {
        switch (lightMode)
        {
            case LightMode.None:
                ToggleNearLights(false);
                ToggleFarLights(false);
                break;

            case LightMode.Near:
                ToggleNearLights(true);
                ToggleFarLights(false);
                break;

            case LightMode.Far:
                ToggleNearLights(false);
                ToggleFarLights(true);
                break;

            default:
                break;
        }
    }

    private void ToggleNearLights(bool isEnabled)
    {
        foreach (var light in _nearLights)
        {
            light.enabled = isEnabled;
        }
    }

    private void ToggleFarLights(bool isEnabled)
    {
        foreach (var light in _farLights)
        {
            light.enabled = isEnabled;
        }
    }
}
