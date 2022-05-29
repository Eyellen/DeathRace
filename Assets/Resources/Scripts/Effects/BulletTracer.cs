using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    private Transform _thisTransform;
    private Vector3 _startPosition;
    [SerializeField] private Vector3 _destination;
    [SerializeField] private float _reachesDestinationInSeconds;
    private float _currentSeconds;

    public Vector3 Destination { get { return _destination; } set { _destination = value; } }

    void Start()
    {
        _thisTransform = transform;
        _startPosition = transform.position;
    }

    void Update()
    {
        UpdatePosition();
        CheckForDestroy();
    }

    private void UpdatePosition()
    {
        _thisTransform.position = Vector3.Lerp(_startPosition, _destination, _currentSeconds / _reachesDestinationInSeconds);
        _currentSeconds += Time.deltaTime;
    }

    private void CheckForDestroy()
    {
        if (_currentSeconds <= _reachesDestinationInSeconds) return;

        Destroy(gameObject);
    }
}
