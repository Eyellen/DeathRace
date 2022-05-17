using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBase : MonoBehaviour
{
    private PlayerInput _input;

    [Header("Wheel visuals")]
    [SerializeField] protected Transform _frontRightWheel;
    [SerializeField] protected Transform _frontLeftWheel;
    [SerializeField] protected Transform _rearRightWheel;
    [SerializeField] protected Transform _rearLeftWheel;

    [Header("Wheel colliders")]
    [SerializeField] protected WheelCollider _frontRightWheelCollider;
    [SerializeField] protected WheelCollider _frontLeftWheelCollider;
    [SerializeField] protected WheelCollider _rearRightWheelCollider;
    [SerializeField] protected WheelCollider _rearLeftWheelCollider;

    [Header("Car settings")]
    [SerializeField] protected float _motorForce;
    [SerializeField] protected float _brakeForce;
    [SerializeField] protected float _maxSteerAngle;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        HandleInput();
        UpdateWheelVisuals();
    }

    private void HandleInput()
    {
        // Gas input
        HandleGas(_input.VerticalAxis * _motorForce);

        // Braking
        HandleBrake(_input.Brake * _brakeForce);

        // Steering
        HandleSteering(_input.HorizontalAxis * _maxSteerAngle);
    }

    private void HandleGas(float force)
    {
        _rearRightWheelCollider.motorTorque = force;
        _rearLeftWheelCollider.motorTorque = force;
    }

    private void HandleBrake(float force)
    {
        _rearRightWheelCollider.brakeTorque = force;
        _rearLeftWheelCollider.brakeTorque = force;
        _frontRightWheelCollider.brakeTorque = force;
        _frontLeftWheelCollider.brakeTorque = force;
    }

    private void HandleSteering(float steer)
    {
        _frontRightWheelCollider.steerAngle = steer;
        _frontLeftWheelCollider.steerAngle = steer;
    }

    private void UpdateWheelVisuals()
    {
        
    }
}
