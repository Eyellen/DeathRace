using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : CameraBase
{
    private PlayerInput _input;

    [SerializeField]
    private float _movementSpeed = 20;

    protected override void Awake()
    {
        base.Awake();

        _input = PlayerInput.Instance;
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        HandleMovement();
    }

    private void HandleMovement()
    {
        // Forward/Backward movement
        _cameraTransform.Translate(_input.VerticalAxis * _cameraTransform.forward * _movementSpeed * Time.deltaTime, Space.World);
        // Right/Left movement
        _cameraTransform.Translate(_input.HorizontalAxis * _cameraTransform.right * _movementSpeed * Time.deltaTime, Space.World);
    }
}