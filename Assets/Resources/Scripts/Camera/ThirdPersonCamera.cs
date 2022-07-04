using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : CameraBase
{
    [SerializeField]
    public Transform Target { get; set; }

    [SerializeField]
    private Vector3 _cameraOffset = new Vector3(0, 1, -5);

    private Vector3 _currentCameraOffset;

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (Target == null)
            GetComponent<CameraManager>().SetFreeCamera();

        HandleFollowing();
    }

    protected override void HandleRotation()
    {
        base.HandleRotation();

        _currentCameraOffset = _thisTransform.rotation * _cameraOffset;
    }

    private void HandleFollowing()
    {
        _thisTransform.position = Target.position + _currentCameraOffset;

#if UNITY_EDITOR || DEBUG_BUILD
        if (_debugging)
        {
            Debug.DrawLine(Target.position, Target.position + _currentCameraOffset);
        }
#endif
    }
}