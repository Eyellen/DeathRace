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

    protected Transform _thisTransform;

    [SerializeField]
    protected Vector2 _sensitivity = new Vector2(1, 1);
    protected const float _sensitivityMultiplier = 6;

    private float _xRotation;
    private float _yRotation;

    protected virtual void Awake()
    {
        _xRotation = transform.rotation.eulerAngles.y;
        _yRotation = -transform.rotation.eulerAngles.x;

        _thisTransform = GetComponent<Transform>();

        UpdateSensitivity();
        SettingsUser.OnSensitivityChanged += UpdateSensitivity;
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

        _thisTransform.rotation = rotation;
    }

    private void UpdateSensitivity()
    {
        _sensitivity = SettingsUser.Sensitivity * _sensitivityMultiplier;
    }
}
