using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Wheel))]
public class WheelEffects : MonoBehaviour
{
    private Wheel _wheel;
    [SerializeField] private TrailRenderer _tireMark;
    [SerializeField] private float _forwardSlipThreshold;
    [SerializeField] private float _sidewaysSlipThreshold;

    private void Start()
    {
        _wheel = GetComponent<Wheel>();
    }

    void Update()
    {
        CheckSlip();
    }

    private void CheckSlip()
    {
        if(Mathf.Abs(_wheel.ForwardSlip) > _forwardSlipThreshold || Mathf.Abs(_wheel.SidewaysSlip) > _sidewaysSlipThreshold)
        {
            StartEmittingTireMark();
            return;
        }
        StopEmittingTireMark();
    }

    private void StartEmittingTireMark()
    {
        if (_tireMark.emitting) return;

        _tireMark.emitting = true;
    }

    private void StopEmittingTireMark()
    {
        if (!_tireMark.emitting) return;

        _tireMark.emitting = false;
    }
}
