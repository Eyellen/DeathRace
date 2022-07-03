using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : CameraBase
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector3 _cameraOffset = new Vector3(0, 1, -5);

    private Vector3 _currentCameraOffset;

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (_target == null)
            FindTarget();

        HandleFollowing();
    }

    protected override void HandleRotation()
    {
        base.HandleRotation();

        _currentCameraOffset = _thisTransform.rotation * _cameraOffset;
    }

    private void HandleFollowing()
    {
#if UNITY_EDITOR || DEBUG_BUILD
        if (!_target)
        {
            if (_debugging)
            {
                Debug.LogWarning("Camera doesn't have target.");
            }
            return;
        }
#else
        if (!_target) return;
#endif

        _thisTransform.position = _target.position + _currentCameraOffset;

#if UNITY_EDITOR || DEBUG_BUILD
        if (_debugging)
        {
            Debug.DrawLine(_target.position, _target.position + _currentCameraOffset);
        }
#endif
    }

    private void FindTarget()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        foreach (var car in cars)
        {
            if (!car.TryGetComponent(out CarBase carBase))
            {
#if UNITY_EDITOR
                Debug.LogError("Trying to GetComponent<CarBase> on object that doesnt contain it. " +
                    "Most likely you forgot to disable \"Player\" tag on Destroyed Car");
#endif
                continue;
            }

            if (!carBase.isLocalPlayer) continue;

            _target = car.transform;
            break;
        }
    }
}