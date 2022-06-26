using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloatingInfo : MonoBehaviour
{
    private Transform _thisTransform;
    private Camera _camera;
    public Transform PlayerTransform { get; set; }
    public float VerticalOffset { get; set; }

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        UpdatePositionOnScreen();
    }

    private void UpdatePositionOnScreen()
    {
        _thisTransform.position = _camera.WorldToScreenPoint(PlayerTransform.position + Vector3.up * VerticalOffset);
    }
}
