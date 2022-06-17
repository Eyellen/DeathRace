using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraBase : NetworkBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    protected bool _debugging = false;
#endif

    protected Transform _cameraTransform;

    [SerializeField]
    protected Vector2 _sensitivity;

    private float _xRotation;
    private float _yRotation;

    protected virtual void Awake()
    {
        _cameraTransform = GetComponent<Transform>();
    }

    protected virtual void LateUpdate()
    {
        HandleRotation();
    }

    protected virtual void HandleRotation()
    {
        _xRotation += Input.GetAxis("Mouse X") * _sensitivity.x;
        _yRotation += Input.GetAxis("Mouse Y") * _sensitivity.y;

        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);
        Quaternion rotation = Quaternion.Euler(-_yRotation, _xRotation, 0);

        _cameraTransform.rotation = rotation;
    }
}
