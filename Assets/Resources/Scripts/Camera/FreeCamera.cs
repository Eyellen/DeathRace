using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : CameraBase
{
    [Header("Free Camera Settings")]
    [SerializeField]
    private float _movementSpeed = 20;

    [SerializeField]
    private float _shiftMultiplier = 3;

    private float _speedMultiplier = 1;
    [field: SerializeField]
    private float MinSpeedMultiplier { get; set; } = 0.05f;

    [field: SerializeField]
    private float MaxSpeedMultiplier { get; set; } = 3f;

    [field: Header("Smooth Camera Settings")]
    private Vector3 Destination { get; set; }

    [field: SerializeField]
    public float SpeedSmoothness { get; set; } = 2f;

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (IsSmoothCamera)
            HandleSmoothMovement();
        else
            HandleMovement();

        HandleMouseScroll();
    }

    private void HandleMouseScroll()
    {
        _speedMultiplier += Input.mouseScrollDelta.y * 0.1f;
        _speedMultiplier = Mathf.Clamp(_speedMultiplier, MinSpeedMultiplier, MaxSpeedMultiplier);
    }

    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _movementSpeed *= _shiftMultiplier;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            _movementSpeed /= _shiftMultiplier;

        // Forward/Backward movement
        _thisTransform.Translate(PlayerInput.VerticalAxis * _movementSpeed * Time.deltaTime * _speedMultiplier * _thisTransform.forward, Space.World);
        // Right/Left movement
        _thisTransform.Translate(PlayerInput.HorizontalAxis * _movementSpeed * Time.deltaTime * _speedMultiplier * _thisTransform.right, Space.World);
        // Up/Down movement
        _thisTransform.Translate(PlayerInput.UpDownAxis * _movementSpeed * Time.deltaTime * _speedMultiplier * _thisTransform.up, Space.World);

        // Updating destination
        Destination = _thisTransform.position;
    }

    private void HandleSmoothMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _movementSpeed *= _shiftMultiplier;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            _movementSpeed /= _shiftMultiplier;

        // Forward/Backward destination
        Destination += PlayerInput.VerticalAxis * _movementSpeed * Time.deltaTime * _speedMultiplier * _thisTransform.forward;
        // Right/Left destination
        Destination += PlayerInput.HorizontalAxis * _movementSpeed * Time.deltaTime * _speedMultiplier * _thisTransform.right;
        // Up/Down destination
        Destination += PlayerInput.UpDownAxis * _movementSpeed * Time.deltaTime * _speedMultiplier * _thisTransform.up;

        _thisTransform.position = Vector3.Lerp(_thisTransform.position, Destination, Time.deltaTime * SpeedSmoothness);
    }
}