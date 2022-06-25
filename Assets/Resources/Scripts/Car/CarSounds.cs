using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    [SerializeField] private CarBase _car;

    [Header("Engine sound")]
    [SerializeField] private float _gasBonus;
    [SerializeField] private AudioSource _engineSource;
    [SerializeField] private float _minEnginePitch, _maxEnginePitch;
    private float _currentGasBonus;

    [Header("Wheel slip sound")]
    [SerializeField] private AudioSource _wheelSlipSource;

    void Start()
    {

    }


    void Update()
    {
        EngineSound();
        WheelSlipSound(_car.MeanForwardSlip, _car.MeanSidewaysSlip);
    }

    private void EngineSound()
    {
        //float pitch = Mathf.Clamp(Mathf.Abs(rpm) / (38197f / 15), _minEnginePitch, _maxEnginePitch);

        _currentGasBonus = _car.IsGasing ? Mathf.Lerp(_currentGasBonus, _gasBonus, Time.deltaTime) : Mathf.Lerp(_currentGasBonus, 0, Time.deltaTime);

        float pitch = Mathf.Clamp((Mathf.Abs(_car.CurrentSpeed) / _car.SpeedLimit) * _maxEnginePitch, _minEnginePitch, _maxEnginePitch - _gasBonus);
        _engineSource.pitch = pitch + _currentGasBonus;
    }

    private void WheelSlipSound(float forwardSlip, float sidewaysSlip)
    {
        float sidewaysSlipVolume = Mathf.Abs(sidewaysSlip) >= 0.1f ? Mathf.Abs(sidewaysSlip) : 0;
        float forwardSlipVolume = Mathf.Abs(forwardSlip) >= 0.5f ? Mathf.Abs(forwardSlip) : 0;

        float volume = Mathf.Max(sidewaysSlipVolume, forwardSlipVolume);

        _wheelSlipSource.volume = volume;
    }
}
