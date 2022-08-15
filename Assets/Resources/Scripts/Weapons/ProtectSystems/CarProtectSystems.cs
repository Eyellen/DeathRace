using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarProtectSystems : NetworkBehaviour
{
    [SerializeField]
    private ParticleSystem _protectionSmoke;

    [SerializeField]
    private float _actionTime = 3;

    [SerializeField]
    private float _cooldown = 20;

    [SerializeField]
    private int _maxCount = 5;
    private int _currentCount;

    private bool _isReady= true;

    [field: SerializeField]
    [field: SyncVar]
    public bool IsActivated { get; set; }

    private IEnumerator _handleSmokeCoroutine;

    private void Start()
    {
        _currentCount = _maxCount;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (!hasAuthority) return;

        if (!IsActivated) return;

        if (PlayerInput.IsSmokePressed && _isReady)
        {
            _currentCount--;
            _handleSmokeCoroutine = HandleSmokeCoroutine(_actionTime);
            StartCoroutine(_handleSmokeCoroutine);
        }
    }

    private IEnumerator HandleSmokeCoroutine(float seconds)
    {
        _isReady = false;
        _protectionSmoke.Play();
        CmdPlayProtectionSmoke(true);

        yield return new WaitForSeconds(seconds);

        _protectionSmoke.Stop();
        CmdPlayProtectionSmoke(false);
        StartCoroutine(HandleCooldownCoroutine(_cooldown));
    }

    private IEnumerator HandleCooldownCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        _isReady = true;
    }

    [Command(requiresAuthority = false)]
    private void CmdPlayProtectionSmoke(bool isPlaying)
    {
        RpcPlayProtectionSmoke(isPlaying);
    }

    [ClientRpc]
    private void RpcPlayProtectionSmoke(bool isPlaying)
    {
        if (isPlaying)
            _protectionSmoke.Play();
        else
            _protectionSmoke.Stop();
    }
}
