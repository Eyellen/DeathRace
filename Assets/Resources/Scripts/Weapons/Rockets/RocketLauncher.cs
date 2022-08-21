using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RocketLauncher : NetworkBehaviour
{
    private Transform _thisTransform;

    [SerializeField] private GameObject[] _rockets;
    [SerializeField] private GameObject _launchedRocketPrefab;
    [SerializeField] private float _timeBetweenLaunches;
    [SyncVar] private float _lastLaunchTime;

    private Vector3 _previousPosition;
    private Vector3 _currentPosition;
    private float _currentMovingSpeed;

    [field: SerializeField]
    [field: SyncVar]
    public bool IsActivated { get; set; }

    public bool IsRocketsRanOut
    {
        get
        {
            foreach (var rocket in _rockets)
            {
                if (rocket != null) return false;
            }

            return true;
        }
    }

    public bool IsReadyToShoot
    {
        get
        {
            return Time.time > _lastLaunchTime + _timeBetweenLaunches;
        }
    }

    void Start()
    {
        _thisTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (!hasAuthority) return;

        HandleInput();
    }

    private void FixedUpdate()
    {
        MeasureSpeed();
    }

    //[ServerCallback]
    private void MeasureSpeed()
    {
        _previousPosition = _currentPosition;
        _currentPosition = _thisTransform.position;

        _currentMovingSpeed = (_currentPosition - _previousPosition).magnitude / Time.deltaTime;
    }

    [ClientCallback]
    private void HandleInput()
    {
        if (!IsActivated) return;

        if (PlayerInput.IsRightActionPressed)
            CmdLaunch(_currentMovingSpeed);
    }

    [Command(requiresAuthority = false)]
    private void CmdLaunch(float rocketSpeed)
    {
        if (!IsActivated) return;

        if (!IsReadyToShoot) return;
        _lastLaunchTime = Time.time;

        for (int i = 0; i < _rockets.Length; i++)
        {
            if (_rockets[i] == null) continue;

            GameObject launchedRocket = Instantiate(_launchedRocketPrefab, _rockets[i].transform.position, _rockets[i].transform.rotation);
            //launchedRocket.GetComponent<Rocket>().Speed += _currentMovingSpeed;
            launchedRocket.GetComponent<Rocket>().Speed += rocketSpeed;
            NetworkServer.Spawn(launchedRocket);

            RpcDestroyRocket(i);

            return;
        }
    }

    [ClientRpc]
    private void RpcDestroyRocket(int rocketIndex)
    {
        Destroy(_rockets[rocketIndex]);
    }
}
