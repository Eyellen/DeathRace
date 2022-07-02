using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : CameraBase
{
    [SerializeField]
    private float _movementSpeed = 20;

    protected override void Awake()
    {
        base.Awake();
    }

    private void HandleMovement()
    {
        // Forward/Backward movement
        _cameraTransform.Translate(PlayerInput.VerticalAxis * _movementSpeed * Time.deltaTime * _cameraTransform.forward, Space.World);
        // Right/Left movement
        _cameraTransform.Translate(PlayerInput.HorizontalAxis * _movementSpeed * Time.deltaTime * _cameraTransform.right, Space.World);
    }
}