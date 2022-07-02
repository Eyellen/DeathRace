using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraBase : NetworkBehaviour
{
#if UNITY_EDITOR || DEBUG_BUILD
    [SerializeField]
    protected bool _debugging = false;
#endif

    protected Transform _cameraTransform;

    [SerializeField]
    protected Vector2 _sensitivity = new Vector2(3, 3);

    private float _xRotation;
    private float _yRotation;

    protected virtual void Awake()
    {
        _cameraTransform = GetComponent<Transform>();
        SettingsUser.OnSensitivityChanged += ChangeSensitivity;
    }

    protected virtual void LateUpdate()
    {
        HandleRotation();
    }

    protected virtual void HandleRotation()
    {
        _xRotation += PlayerInput.MouseHorizontalAxis * _sensitivity.x;
        _yRotation += PlayerInput.MouseVerticalAxis * _sensitivity.y;

        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);
        Quaternion rotation = Quaternion.Euler(-_yRotation, _xRotation, 0);

        _cameraTransform.rotation = rotation;
    }

    private void ChangeSensitivity()
    {
        _sensitivity = SettingsUser.Sensitivity * 6;
    }
}
