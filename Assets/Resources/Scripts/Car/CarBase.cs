using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBase : MonoBehaviour
{
    [Serializable]
    public class Wheel
    {
        public Transform Transform;
        public WheelCollider Collider;
    }
    public enum AxelLocation
    {
        Front,
        Rear
    }
    [Serializable]
    public class Axel
    {
        public AxelLocation AxelLocation;
        public bool IsDriveWheel;
        [Range(0, 1)] public float BrakesInfluence;
        public Wheel RightWheel;
        public Wheel LeftWheel;
    }

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
    public float SidewaysSlip
    {
        get
        {
            float slip = 0;
            int wheelsChecked = 0;

            WheelHit hitInfo;
            foreach (var axel in _axels)
            {
                if (axel.RightWheel.Collider.GetGroundHit(out hitInfo))
                {
                    slip += hitInfo.sidewaysSlip;
                    wheelsChecked++;
                }
                if (axel.LeftWheel.Collider.GetGroundHit(out hitInfo))
                {
                    slip += hitInfo.sidewaysSlip;
                    wheelsChecked++;
                }
            }
            return slip / wheelsChecked;
        }
    }
    public float ForwardSlip
    {
        get
        {
            float slip = 0;
            int wheelsChecked = 0;

            WheelHit hitInfo;
            foreach (var axel in _axels)
            {
                if (axel.RightWheel.Collider.GetGroundHit(out hitInfo))
                {
                    slip += hitInfo.forwardSlip;
                    wheelsChecked++;
                }
                if (axel.LeftWheel.Collider.GetGroundHit(out hitInfo))
                {
                    slip += hitInfo.forwardSlip;
                    wheelsChecked++;
                }
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
        UpdateWheelVisuals();

        Debug.Log(Rpm);
    }

    private void HandleInput()
    {
        // Gas input
        HandleGas(_input.VerticalAxis * _motorForce);

        // Braking
        HandleBrake(_input.Brake * _brakeForce);
        ApplyEngineBraking((Mathf.Abs(_input.VerticalAxis) >= 0.1f), (Mathf.Abs(_input.Brake) >= 0.1f));

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

    private void UpdateWheelVisuals()
    {
        foreach (var axel in _axels)
        {
            UpdateSingleWheel(axel.RightWheel.Collider, axel.RightWheel.Transform);
            UpdateSingleWheel(axel.LeftWheel.Collider, axel.LeftWheel.Transform);
        }
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
