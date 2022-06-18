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

        StartCoroutine(FindTarget());

        HandleFollowing();
    }

    protected override void HandleRotation()
    {
        base.HandleRotation();

        _currentCameraOffset = _cameraTransform.rotation * _cameraOffset;
    }

    private void HandleFollowing()
    {
#if UNITY_EDITOR
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

        _cameraTransform.position = _target.position + _currentCameraOffset;

#if UNITY_EDITOR
        if (_debugging)
        {
            Debug.DrawLine(_target.position, _target.position + _currentCameraOffset);
        }
#endif
    }

    private IEnumerator FindTarget()
    {
        while (!_target)
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