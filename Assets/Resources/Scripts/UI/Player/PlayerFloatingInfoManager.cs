using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerFloatingInfoManager : NetworkBehaviour
{
    private Transform _thisTransform;
    private Transform _cameraTransform;
    private GameObject _playerFloatingInfo;
    [SerializeField] private GameObject _playerFloatingInfoPrefab;
    [SerializeField] private float _visibleDistance;
    [SerializeField] private float _verticalOffset = 1.5f;

    private GameObject _canvas;

    [field: SyncVar]
    [field: SerializeField]
    public string Username { get; set; }

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();
        _cameraTransform = Camera.main.transform;
        _canvas = GameObject.Find("Canvas");

        InitializeFloatingInfo();
    }

    private void Update()
    {
        CheckIfNeedToShowOrHide();
    }

    private void OnDestroy()
    {
        Destroy(_playerFloatingInfo);
    }

    private void CheckIfNeedToShowOrHide()
    {
        Vector3 player = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = player.z > 0 && (player.x > -0.1 && player.x < 1.1) && (player.y > -0.1 && player.y < 1.1);

        if (Vector3.Distance(_cameraTransform.position, _thisTransform.position) > _visibleDistance || !onScreen)
        {
            HideFloatingInfo();
            return;
        }
        ShowFloatingInfo();
    }

    private void ShowFloatingInfo()
    {
        _playerFloatingInfo.SetActive(true);
    }

    private void HideFloatingInfo()
    {
        _playerFloatingInfo.SetActive(false);
    }

    private void InitializeFloatingInfo()
    {
        _playerFloatingInfo = Instantiate(_playerFloatingInfoPrefab, _canvas.transform);

        PlayerFloatingInfo floatingInfo = _playerFloatingInfo.GetComponent<PlayerFloatingInfo>();
        floatingInfo.PlayerTransform = transform;
        floatingInfo.VerticalOffset = _verticalOffset;
        floatingInfo.Username = Username;

        HideFloatingInfo();
    }
}
