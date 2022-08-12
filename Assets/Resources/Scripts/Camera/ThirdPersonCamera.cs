using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : CameraBase
{
    [SerializeField]
    public Transform Target { get; set; }
    private Rigidbody _targetRigidbody;

    [SerializeField]
    private Vector3 _cameraOffset = new Vector3(0, 1, -5);

    [SerializeField]
    private float _mouseInactiveThreshold = 0.05f;

    [SerializeField]
    private float _switchToAutoCameraAfterSeconds = 3f;

    [SerializeField]
    private float _autoCameraLookSmoothness = 2;

    private Vector3 _currentCameraOffset;

    private float _lastMouseInputTime;

    private void Start()
    {
        _targetRigidbody = Target.GetComponent<Rigidbody>();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (Target == null)
        {
            GetComponent<CameraManager>().SetFreeCamera();
            return;
        }

        if (Mathf.Abs(PlayerInput.MouseHorizontalAxis) >= _mouseInactiveThreshold &&
            Mathf.Abs(PlayerInput.MouseVerticalAxis) >= _mouseInactiveThreshold)
        {
            _lastMouseInputTime = Time.time;
        }

        HandleFollowing();
    }

    protected override void HandleRotation()
    {
        if (PlayerInput.IsBackViewHolding)
        {
            HandleBackView();
            _currentCameraOffset = _thisTransform.rotation * _cameraOffset;
            return;
        }
        if(PlayerInput.IsReleasedBackView)
        {
            _thisTransform.rotation = Quaternion.Euler(20, Target.rotation.eulerAngles.y, 0);
        }

        // Would be good to implement state machine here
        if (_lastMouseInputTime + _switchToAutoCameraAfterSeconds > Time.time ||
            Mathf.Abs(_targetRigidbody.velocity.magnitude) < 1f)
        {
            base.HandleRotation();
        }
        else
        {
            HandleAutoCamera();
        }

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

    private void HandleAutoCamera()
    {
        _thisTransform.rotation = Quaternion.Lerp(_thisTransform.rotation,
            Quaternion.Euler(20, Target.rotation.eulerAngles.y, 0),
            Time.deltaTime * _autoCameraLookSmoothness);

        XRotation = _thisTransform.rotation.eulerAngles.y;
        YRotation = -_thisTransform.rotation.eulerAngles.x;
    }

    private void HandleBackView()
    {
        _thisTransform.rotation = Quaternion.Euler(20, Target.rotation.eulerAngles.y + 180, 0);
    }
}