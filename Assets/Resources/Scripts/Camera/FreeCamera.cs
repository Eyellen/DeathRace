using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : CameraBase
{
    [SerializeField]
    private float _movementSpeed = 20;

    protected override void LateUpdate()
    {
        base.LateUpdate();

        HandleMovement();
    }

    private void HandleMovement()
    {
        // Forward/Backward movement
        _thisTransform.Translate(PlayerInput.VerticalAxis * _movementSpeed * Time.deltaTime * _thisTransform.forward, Space.World);
        // Right/Left movement
        _thisTransform.Translate(PlayerInput.HorizontalAxis * _movementSpeed * Time.deltaTime * _thisTransform.right, Space.World);
        // Up/Down movement
        _thisTransform.Translate(PlayerInput.UpDownAxis * _movementSpeed * Time.deltaTime * _thisTransform.up, Space.World);
    }
}