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

    [Header("Wheels")]
    [SerializeField] protected Axel[] _axels;

    #region Properties
    public bool IsGrounded
    {
        get
        {
            foreach (var axel in _axels)
            {
                if (axel.RightWheel.Collider.isGrounded || axel.LeftWheel.Collider.isGrounded) return true;
            }
            return false;
        }
    }
    public float Rpm
    {
        get
        {
            float rpm = 0;
            int wheelsSchecked = 0;
            foreach (var axel in _axels)
            {
                rpm += axel.RightWheel.Collider.rpm + axel.LeftWheel.Collider.rpm;
                wheelsSchecked += 2;
            }
            return rpm / wheelsSchecked;
        }
    }
    public float MeanSidewaysSlip
    {
        get
        {
            float slip = 0;
            int wheelsChecked = 0;

            foreach (var axel in _axels)
            {
                slip += axel.RightWheel.SidewaysSlip;
                slip += axel.LeftWheel.SidewaysSlip;
                wheelsChecked += 2;
            }
            return slip / wheelsChecked;
        }
    }
    public float MeanForwardSlip
    {
        get
        {
            float slip = 0;
            int wheelsChecked = 0;

            foreach (var axel in _axels)
            {
                slip += axel.RightWheel.ForwardSlip;
                slip += axel.LeftWheel.ForwardSlip;
                wheelsChecked += 2;
            }
            return slip / wheelsChecked;
        }
    }
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _centreOfMass.localPosition;
        _input = PlayerInput.Instance;
    }

    private void FixedUpdate()
    {
        HandleInput();
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
        foreach (var axel in _axels)
        {
            if(axel.IsDriveWheel)
            {
                axel.RightWheel.Collider.motorTorque = force;
                axel.LeftWheel.Collider.motorTorque = force;
            }
        }
    }

    private void HandleBrake(float force)
    {
        if(force > 0.1f)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * Mathf.Lerp(_rigidbody.velocity.magnitude, 0f, 0.007f);
        }
        foreach (var axel in _axels)
        {
            axel.RightWheel.Collider.brakeTorque = force * axel.BrakesInfluence;
            axel.LeftWheel.Collider.brakeTorque = force * axel.BrakesInfluence;
        }
    }

    private void ApplyEngineBraking(bool isRavs, bool isBraking)
    {
        //if (isRavs) return;
        //if (isBraking) return;

        //_rigidbody.velocity = _rigidbody.velocity.normalized * Mathf.Lerp(0, _rigidbody.velocity.magnitude, 0.99f);
        //Debug.Log("Engine Braking is being applied !");
    }

    private void HandleSteering(float steer)
    {
        foreach (var axel in _axels)
        {
            if(axel.AxelLocation == AxelLocation.Front)
            {
                axel.RightWheel.Collider.steerAngle = Mathf.Lerp(axel.RightWheel.Collider.steerAngle, steer, 0.5f);
                axel.LeftWheel.Collider.steerAngle = Mathf.Lerp(axel.LeftWheel.Collider.steerAngle, steer, 0.5f);
            }
        }
    }
}
