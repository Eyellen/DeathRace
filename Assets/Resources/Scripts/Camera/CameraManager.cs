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

    private IEnumerator _setFreeCameraCoroutine;

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

    public void SetThirdPersonCamera(Transform target)
    {
        if (_setFreeCameraCoroutine != null)
            StopCoroutine(_setFreeCameraCoroutine);

        CameraMode = CameraMode.ThirdPerson;
        ((ThirdPersonCamera)_currentCameraScript).Target = target;
    }

    public void SetFreeCamera()
    {
        CameraMode = CameraMode.Free;
    }

    public void SetFreeCamera(float seconds)
    {
        if (_setFreeCameraCoroutine != null)
            StopCoroutine(_setFreeCameraCoroutine);

        _setFreeCameraCoroutine = SetFreeCameraCoroutine(seconds);
        StartCoroutine(_setFreeCameraCoroutine);
    }

    private IEnumerator SetFreeCameraCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CameraMode = CameraMode.Free;
    }
}
