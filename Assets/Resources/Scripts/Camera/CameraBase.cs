using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBase : MonoBehaviour
{
    private Transform _camera;

    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] float _distanceToTarget;
    [SerializeField] private Vector2 _sensitivity;

    private float _xRotation;
    private float _yRotation;
    private Transform _cameraAnchor;

    private void Awake()
    {
        _camera = GetComponent<Transform>();
    }

    private void Start()
    {
        InitializeCamera();
    }

    private void LateUpdate()
    {
        HandleFollowing();
        HandleRotation();
    }

    private void HandleRotation()
    {
        _xRotation += Input.GetAxis("Mouse X") * _sensitivity.x;
        _yRotation += Input.GetAxis("Mouse Y") * _sensitivity.y;

        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);
        Quaternion rotation = Quaternion.Euler(-_yRotation, _xRotation, 0);

        _cameraAnchor.rotation = rotation;
    }

    private void HandleFollowing()
    {
        //_camera.position = _target.position + _cameraOffset;
        _cameraAnchor.position = _target.position;
    }

    private void InitializeCamera()
    {
        _cameraAnchor = new GameObject("CameraAnchor").transform;
        _camera.parent = _cameraAnchor;
        _camera.localPosition = _cameraOffset;
    }
}
