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
        _car.OnEngineWork += PlayEngineSound;
        _car.OnWheelSlip += PlaySidewaysSlipSound;
    }


    void Update()
    {
        
    }

    private void PlayEngineSound(float rpm)
    {
        float pitch = Mathf.Clamp(Mathf.Abs(rpm) / (38197f / 20), _minEnginePitch, _maxEnginePitch);
        _engineSource.pitch = pitch;
    }

    private void PlaySidewaysSlipSound(WheelHit hitInfo)
    {
        float sidewaysSlipVolume = Mathf.Abs(hitInfo.sidewaysSlip) >= 0.1f ? Mathf.Abs(hitInfo.sidewaysSlip) : 0;
        float forwardSlipVolume = Mathf.Abs(hitInfo.forwardSlip) >= 0.5f ? Mathf.Abs(hitInfo.forwardSlip) : 0;

        float volume = Mathf.Max(sidewaysSlipVolume, forwardSlipVolume);

        _wheelSlipSource.volume = volume;
    }
}
