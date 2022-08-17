using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarLights : NetworkBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _offLightsMaterial;
    [SerializeField] private Material _onLightsMaterial;

    [Header("Lights objects")]
    [SerializeField] private Renderer[] _lightsRenderer;
    [SerializeField] private Renderer[] _additionalLightsRenderer;

    [field: Header("Parameters")]
    [field: SerializeField]
    [field: Range(0, 2)]
    [field: SyncVar(hook = nameof(ToggleLights))]
    public int CurrentLightModeIndex { get; private set; } = 0;

    [Header("Near Lights")]
    [SerializeField]
    private Light[] _nearLights;

    [Header("Far Lights")]
    [SerializeField]
    private Light[] _farLights;

    private void Start()
    {
        InitializeLights();
    }

    private void Update()
    {
        if (!hasAuthority) return;

        if(PlayerInput.IsLightsPressed)
        {
            CurrentLightModeIndex = (CurrentLightModeIndex + 1) % Enum.GetValues(typeof(LightMode)).Length;
            ToggleLights(0, CurrentLightModeIndex);
            CmdSetCurrentLightModeIndex(CurrentLightModeIndex);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetCurrentLightModeIndex(int lightModeIndex)
    {
        CurrentLightModeIndex = lightModeIndex;
    }

    private void ToggleLights(int prevIndex, int newIndex)
    {
        foreach (var lightsObjects in _lightsRenderer)
        {
            lightsObjects.material = newIndex > 0 ? _onLightsMaterial : _offLightsMaterial;
        }
        switch ((LightMode)newIndex)
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
        foreach (var lightsObjects in _additionalLightsRenderer)
        {
            lightsObjects.material = isEnabled ? _onLightsMaterial : _offLightsMaterial;
        }
    }

    private void InitializeLights()
    {
        ToggleLights(0, CurrentLightModeIndex);
    }
}
