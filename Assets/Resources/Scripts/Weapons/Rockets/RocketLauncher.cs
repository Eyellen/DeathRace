using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    private PlayerInput _input;

    [SerializeField] private GameObject[] _rockets;
    [SerializeField] private GameObject _launchedRocketPrefab;
    [SerializeField] private float _timeBetweenLaunches;
    private float _lastLaunchTime;

    void Start()
    {
        _input = PlayerInput.Instance;
    }


    void Update()
    {
        HandleInput();
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

            Instantiate(_launchedRocketPrefab, rocket.transform.position, rocket.transform.rotation);
            Destroy(rocket);

            return;
        }
    }
}
