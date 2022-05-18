using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBase : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private PlayerInput _input;

    [Header("Car settings")]
    [SerializeField] protected float _motorForce;
    [SerializeField] protected float _brakeForce;
    [SerializeField] protected float _maxSteerAngle;
    [SerializeField] protected Transform _centreOfMass;

    [Serializable]
    public class Wheel
    {
        public Transform transform;
        public WheelCollider collider;
    }

    [Header("Wheels")]
    [SerializeField] protected Wheel _frontRightWheel;
    [SerializeField] protected Wheel _frontLeftWheel;
    [SerializeField] protected Wheel _rearRightWheel;
    [SerializeField] protected Wheel _rearLeftWheel;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        //_rigidbody.centerOfMass = _centreOfMass.localPosition;
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
        _frontRightWheel.collider.motorTorque = force;
        _frontLeftWheel.collider.motorTorque = force;

        _rearRightWheel.collider.motorTorque = force;
        _rearLeftWheel.collider.motorTorque = force;
    }

    private void HandleBrake(float force)
    {
        if(force > 0.1f)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * Mathf.Lerp(_rigidbody.velocity.magnitude, 0f, 0.007f);
        }
        _rearRightWheel.collider.brakeTorque = force;
        _rearLeftWheel.collider.brakeTorque = force;
        _frontRightWheel.collider.brakeTorque = force;
        _frontLeftWheel.collider.brakeTorque = force;
    }

    private void HandleSteering(float steer)
    {
        _frontRightWheel.collider.steerAngle = Mathf.Lerp(_frontRightWheel.collider.steerAngle, steer, 0.5f);
        _frontLeftWheel.collider.steerAngle = Mathf.Lerp(_frontLeftWheel.collider.steerAngle, steer, 0.5f);
    }

    private void UpdateWheelVisuals()
    {
        UpdateSingleWheel(_frontRightWheel.collider, _frontRightWheel.transform);
        UpdateSingleWheel(_frontLeftWheel.collider, _frontLeftWheel.transform);

        UpdateSingleWheel(_rearRightWheel.collider, _rearRightWheel.transform);
        UpdateSingleWheel(_rearLeftWheel.collider, _rearLeftWheel.transform);
    }
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheel)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheel.position = position;
        wheel.rotation = rotation;
    }
}
