using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CameraMode _cameraMode;

    private GameObject _camera;

    private CameraBase _currentCameraScript;

    public CameraBase CurrentCameraScript { get => _currentCameraScript; }

    public CameraMode CameraMode
    {
        get => _cameraMode;

        set
        {
            if (_cameraMode == value) return;

            CameraModeChanged(value);
            _cameraMode = value;
        }
    }

    private void Awake()
    {
        _camera = gameObject;

        Initialize();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            CameraMode = CameraMode.Free;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CameraMode = CameraMode.ThirdPerson;
        }
    }

    private void CameraModeChanged(CameraMode cameraMode)
    {
        switch (cameraMode)
        {
            case CameraMode.Free:
                {
                    Destroy(_currentCameraScript);
                    _currentCameraScript = _camera.AddComponent<FreeCamera>();
                    break;
                }
            case CameraMode.ThirdPerson:
                {
                    Destroy(_currentCameraScript);
                    _currentCameraScript = _camera.AddComponent<ThirdPersonCamera>();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void Initialize()
    {
        if (!TryGetComponent(out _currentCameraScript))
        {
            _currentCameraScript = gameObject.AddComponent<FreeCamera>();
            _cameraMode = CameraMode.Free;
        }

        switch (_currentCameraScript)
        {
            case FreeCamera freeCamera:
                {
                    _cameraMode = CameraMode.Free;
                    break;
                }
            case ThirdPersonCamera carCamera:
                {
                    _cameraMode = CameraMode.ThirdPerson;
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
