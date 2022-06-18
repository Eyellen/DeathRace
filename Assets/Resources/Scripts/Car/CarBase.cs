using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarBase : NetworkBehaviour
{
    protected Transform _thisTransform;
    protected Rigidbody _rigidbody;
    protected PlayerInput _input;

    [Header("Car settings")]
    [SerializeField] protected float _motorForce;
    [SerializeField] protected float _speedLimit;
    [SerializeField] protected float _brakeForce;
    [SerializeField] protected float _maxSteerAngle;
    [SerializeField] protected Transform _centreOfMass;

    [Header("Wheels")]
    [SerializeField] protected Axle[] _axles;

    #region Properties
    public bool IsGrounded
    {
        get
        {
            foreach (var axle in _axles)
            {
                if (axle.RightWheel.Collider.isGrounded || axle.LeftWheel.Collider.isGrounded) return true;
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
            foreach (var axle in _axles)
            {
                rpm += axle.RightWheel.Collider.rpm + axle.LeftWheel.Collider.rpm;
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

            foreach (var axle in _axles)
            {
                slip += axle.RightWheel.SidewaysSlip;
                slip += axle.LeftWheel.SidewaysSlip;
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

            foreach (var axle in _axles)
            {
                slip += axle.RightWheel.ForwardSlip;
                slip += axle.LeftWheel.ForwardSlip;
                wheelsChecked += 2;
            }
            return slip / wheelsChecked;
        }
    }
    #endregion

    private void Awake()
    {
        _thisTransform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = _centreOfMass.localPosition;
        _input = PlayerInput.Instance;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        HandleInput();
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            _rigidbody.velocity = Vector3.zero;
            _thisTransform.position = new Vector3(10f, 1f, 10f);
        }

        // Gas input
        HandleGas(_input.VerticalAxis * _motorForce);

        // Braking
        HandleBrake(_input.Brake * _brakeForce);

        // Steering
        HandleSteering(_input.HorizontalAxis * _maxSteerAngle);
    }

    private void HandleGas(float force)
    {
        foreach (var axle in _axles)
        {
            if (!axle.IsDriveAxle) continue;

            axle.RightWheel.Collider.motorTorque = force;
            axle.LeftWheel.Collider.motorTorque = force;
        }

        if (Mathf.Abs(_rigidbody.velocity.magnitude) > _speedLimit)
        {
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _speedLimit);
        }
    }

    private void HandleBrake(float force)
    {
        if(force > 0.1f)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * Mathf.Lerp(_rigidbody.velocity.magnitude, 0f, 0.007f);
        }
        foreach (var axle in _axles)
        {
            axle.RightWheel.Collider.brakeTorque = force * axle.BrakesInfluence;
            axle.LeftWheel.Collider.brakeTorque = force * axle.BrakesInfluence;
        }
    }

    private void HandleSteering(float steer)
    {
        foreach (var axle in _axles)
        {
            if (axle.AxleLocation != AxleLocation.Front) continue;

            axle.RightWheel.Collider.steerAngle = Mathf.Lerp(axle.RightWheel.Collider.steerAngle, steer, 0.5f);
            axle.LeftWheel.Collider.steerAngle = Mathf.Lerp(axle.LeftWheel.Collider.steerAngle, steer, 0.5f);
        }
    }
}
