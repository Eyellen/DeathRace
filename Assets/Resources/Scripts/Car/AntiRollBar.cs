using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private WheelCollider _rightWheel;
    [SerializeField] private WheelCollider _leftWheel;
    [SerializeField] private float _antiRoll = 5000;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        WheelHit rightHit;
        WheelHit leftHit;
        float rightTravel = 1;
        float leftTravel = 1;

        bool isRightGrounded = _rightWheel.GetGroundHit(out rightHit);
        if(isRightGrounded)
        {
            rightTravel = (-_rightWheel.transform.InverseTransformPoint(rightHit.point).y - _rightWheel.radius) / _rightWheel.suspensionDistance;
        }

        bool isLeftGrounded = _leftWheel.GetGroundHit(out leftHit);
        if (isLeftGrounded)
        {
            leftTravel = (-_leftWheel.transform.InverseTransformPoint(leftHit.point).y - _leftWheel.radius) / _leftWheel.suspensionDistance;
        }

        float antiRollForce = (leftTravel - rightTravel) * _antiRoll;

        if(isRightGrounded)
        {
            _rigidbody.AddForceAtPosition(_rightWheel.transform.up * -antiRollForce, _rightWheel.transform.position);
        }

        if (isLeftGrounded)
        {
            _rigidbody.AddForceAtPosition(_leftWheel.transform.up * -antiRollForce, _leftWheel.transform.position);
        }
    }
}
