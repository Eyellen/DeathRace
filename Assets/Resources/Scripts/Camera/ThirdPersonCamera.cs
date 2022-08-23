using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : CameraBase
{
    [field: Header("Third Person Camera Settings")]
    private Camera _camera;

    private Transform _target;
    [SerializeField]
    public Transform Target
    {
        get => _target;
        set => _target = value;
    }
    public Rigidbody TargetRigidbody { get; set; }

    private float _yMaxRotation = -70;
    private float _yMinRotation = 10;

    protected override float yMaxRotation { get => _yMaxRotation; set => _yMaxRotation = value; }
    protected override float yMinRotation { get => _yMinRotation; set => _yMinRotation = value; }


    [SerializeField]
    private Vector3 _cameraOffset = new Vector3(0, 0.4f, -5);
    private Vector3 _currentCameraOffset;

    [SerializeField]
    private float _minSpeedFov = 60;

    [SerializeField]
    private float _maxSpeedFov = 70;

    [SerializeField]
    private float _maxMovementSpeed = 25;

    private int _layer;

    [field: Header("Auto Camera Settings")]
    [SerializeField]
    private float _mouseInactiveThreshold = 0.05f;

    [SerializeField]
    private float _switchToAutoCameraAfterSeconds = 3f;

    [SerializeField]
    private float _autoCameraLookSmoothness = 2;

    private float _lastMouseInputTime;

    protected override void Awake()
    {
        base.Awake();

        _layer = 1 << LayerMask.NameToLayer("Car");

        _camera = GetComponentInChildren<Camera>();
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

        HandleOffsetMagnitude();
        HandleFollowing();

        HandleFieldOfView();
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
            Mathf.Abs(TargetRigidbody.velocity.magnitude) < 1f)
        {
            base.HandleRotation();
        }
        else
        {
            HandleAutoCamera();
        }

        _currentCameraOffset = _thisTransform.rotation * _cameraOffset;
    }

    private void HandleOffsetMagnitude()
    {
        if (!Physics.SphereCast(Target.position, radius: 0.2f, _currentCameraOffset, out RaycastHit hitInfo, 
            _currentCameraOffset.magnitude, ~_layer, QueryTriggerInteraction.Ignore)) return;

        Vector3 newOffset = hitInfo.point - Target.position;

        _currentCameraOffset = _currentCameraOffset.normalized * newOffset.magnitude;
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

    private void HandleFieldOfView()
    {
        _camera.fieldOfView = Mathf.Lerp(_minSpeedFov, _maxSpeedFov,
            Mathf.SmoothStep(0, 1, Mathf.Abs(TargetRigidbody.velocity.magnitude) / _maxMovementSpeed));
    }
}