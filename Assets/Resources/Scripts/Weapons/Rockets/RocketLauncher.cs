using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    private PlayerInput _input;
    private Transform _thisTransform;

    [SerializeField] private GameObject[] _rockets;
    [SerializeField] private GameObject _launchedRocketPrefab;
    [SerializeField] private float _timeBetweenLaunches;
    private float _lastLaunchTime;

    private Vector3 _previousPosition;
    private Vector3 _currentPosition;
    private float _currentMovingSpeed;

    void Start()
    {
        _input = PlayerInput.Instance;
        _thisTransform = GetComponent<Transform>();
    }

    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MeasureSpeed();
    }

    private void MeasureSpeed()
    {
        _previousPosition = _currentPosition;
        _currentPosition = _thisTransform.position;

        _currentMovingSpeed = (_currentPosition - _previousPosition).magnitude / Time.deltaTime;
    }

    private void HandleInput()
    {
        if (_input.IsRightActionPressed) Launch();
    }

    private void Launch()
    {
        if (Time.time < _lastLaunchTime + _timeBetweenLaunches) return;
        _lastLaunchTime = Time.time;

        foreach (GameObject rocket in _rockets)
        {
            if (!rocket) continue;

            GameObject launchedRocket = Instantiate(_launchedRocketPrefab, rocket.transform.position, rocket.transform.rotation);
            launchedRocket.GetComponent<Rocket>().Speed += _currentMovingSpeed;

            Destroy(rocket);

            return;
        }
    }
}
