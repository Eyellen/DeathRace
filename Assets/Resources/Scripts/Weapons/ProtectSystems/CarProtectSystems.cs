using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarProtectSystems : NetworkBehaviour, IActivatable
{
    [field: Header("Common")]
    [field: SerializeField]
    [field: SyncVar]
    public bool IsActivated { get; set; }

    [Header("Smoke part")]
    [SerializeField]
    private ParticleSystem _protectionSmoke;

    [SerializeField]
    private float _smokeActionTime = 3;

    [SerializeField]
    private float _smokeCooldown = 20;

    [SerializeField]
    private int _maxSmokeCount = 5;
    private int _currentSmokeCount;

    public bool IsSmokeReady { get; private set; } = true;

    private IEnumerator _handleSmokeCoroutine;

    public bool IsSmokeRanOut
    {
        get
        {
            if (_currentSmokeCount <= 0) return true;

            return false;
        }
    }

    private void Start()
    {
        _currentSmokeCount = _maxSmokeCount;
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (!hasAuthority) return;

        if (!IsActivated) return;

        if (PlayerInput.IsSmokePressed && IsSmokeReady)
        {
            _currentSmokeCount--;
            _handleSmokeCoroutine = HandleSmokeCoroutine(_smokeActionTime);
            StartCoroutine(_handleSmokeCoroutine);
        }
    }

    private IEnumerator HandleSmokeCoroutine(float seconds)
    {
        IsSmokeReady = false;
        _protectionSmoke.Play();
        CmdPlayProtectionSmoke(true);

        yield return new WaitForSeconds(seconds);

        _protectionSmoke.Stop();
        CmdPlayProtectionSmoke(false);
        StartCoroutine(HandleCooldownCoroutine(_smokeCooldown));
    }

    private IEnumerator HandleCooldownCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        IsSmokeReady = true;
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
