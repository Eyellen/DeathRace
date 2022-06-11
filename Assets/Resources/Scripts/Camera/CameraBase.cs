using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraBase : NetworkBehaviour
{
    private Transform _camera;

    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private Vector2 _sensitivity;

    private Vector3 _currentCameraOffset;
    private float _xRotation;
    private float _yRotation;

    private void Awake()
    {
        _camera = GetComponent<Transform>();
    }

    private void Start()
    {

    }

    private void LateUpdate()
    {
        StartCoroutine(FindTarget());

        HandleRotation();
        HandleFollowing();
        Debug.DrawLine(_target.position, _target.position + _currentCameraOffset);
    }

    private void HandleRotation()
    {
        _xRotation += Input.GetAxis("Mouse X") * _sensitivity.x;
        _yRotation += Input.GetAxis("Mouse Y") * _sensitivity.y;

        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);
        Quaternion rotation = Quaternion.Euler(-_yRotation, _xRotation, 0);

        _camera.rotation = rotation;
        _currentCameraOffset = rotation * _cameraOffset;
    }

    private void HandleFollowing()
    {
        _camera.position = _target.position + _currentCameraOffset;
    }

    private IEnumerator FindTarget()
    {
        while(!_target)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (var player in players)
            {
                if (!player.GetComponent<CarBase>().isLocalPlayer) continue;

                _target = player.transform;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
