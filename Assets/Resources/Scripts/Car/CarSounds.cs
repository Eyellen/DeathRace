using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    [SerializeField] private CarBase _car;

    [SerializeField] private AudioSource _engineSource;
    [SerializeField] private float _minEnginePitch, _maxEnginePitch;
    [SerializeField] private AudioSource _wheelSlipSource;

    void Start()
    {

    }


    void Update()
    {
        EngineSound(_car.Rpm);
        WheelSlipSound(_car.ForwardSlip, _car.SidewaysSlip);
    }

    private void EngineSound(float rpm)
    {
        float pitch = Mathf.Clamp(Mathf.Abs(rpm) / (38197f / 15), _minEnginePitch, _maxEnginePitch);
        _engineSource.pitch = pitch;
    }

    private void WheelSlipSound(float forwardSlip, float sidewaysSlip)
    {
        float sidewaysSlipVolume = Mathf.Abs(sidewaysSlip) >= 0.1f ? Mathf.Abs(sidewaysSlip) : 0;
        float forwardSlipVolume = Mathf.Abs(forwardSlip) >= 0.5f ? Mathf.Abs(forwardSlip) : 0;

        float volume = Mathf.Max(sidewaysSlipVolume, forwardSlipVolume);

        _wheelSlipSource.volume = volume;
    }
}
