using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarLights : NetworkBehaviour
{
    [SerializeField]
    [Range(0, 2)]
    [SyncVar]
    private int _currentLightModeIndex = 0;
    private int _previousLightModeIndex;

    [Header("Near Lights")]
    [SerializeField]
    private Light[] _nearLights;

    [Header("Far Lights")]
    [SerializeField]
    private Light[] _farLights;

    private void Update()
    {
        if (_currentLightModeIndex != _previousLightModeIndex)
        {
            _previousLightModeIndex = _currentLightModeIndex;
            ToggleLights((LightMode)_currentLightModeIndex);
        }

        if (!hasAuthority) return;

        if(PlayerInput.IsLightsPressed)
        {
            _currentLightModeIndex = (_currentLightModeIndex + 1) % Enum.GetValues(typeof(LightMode)).Length;
            CmdSetCurrentLightModeIndex(_currentLightModeIndex);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetCurrentLightModeIndex(int lightModeIndex)
    {
        _currentLightModeIndex = lightModeIndex;
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
