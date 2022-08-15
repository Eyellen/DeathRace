using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Minigun : GunBase
{
    [Header("Minigun")]
    [SerializeField] private Transform[] _minigunBarrels;

    [Header("Minigun settings")]
    [SerializeField] private float _barrelsSpinningTime;
    private float _spinningTimePassed;
    [SyncVar] private bool _isSpinning;
    [SerializeField] private float _maxSpinningSpeed;
    private float _currentSpinningSpeed;

    #region Properties
    public bool IsSpinning { get => _isSpinning; }
    #endregion

    protected override void Update()
    {
        base.Update();

        if (!hasAuthority)
            SpinBarrels(IsSpinning);
    }

    protected override void HandleInput()
    {
        if (!hasAuthority) return;

        Shoot(PlayerInput.IsLeftActionPressed);
    }

    protected override void Shoot(bool isActionOccurs)
    {
        if (!IsActivated) return;

        if (IsShooting)
            CmdSetShooting(false);

        if (SpinBarrels(isActionOccurs) != _maxSpinningSpeed) return;

        base.Shoot(isActionOccurs);
    }

    private float SpinBarrels(bool isActionOccurs)
    {
        _currentSpinningSpeed = Mathf.Lerp(0, _maxSpinningSpeed, _spinningTimePassed / _barrelsSpinningTime);

        foreach (var barrel in _minigunBarrels)
            barrel.Rotate(Vector3.forward, _currentSpinningSpeed * Time.deltaTime);

        if (!isActionOccurs)
        {
            if (_isSpinning)
                CmdSetIsSpinning(false);
            _spinningTimePassed = Mathf.Clamp(_spinningTimePassed - Time.deltaTime, 0, _barrelsSpinningTime);
            return _currentSpinningSpeed;
        }

        if (!_isSpinning)
            CmdSetIsSpinning(true);
        _spinningTimePassed = Mathf.Clamp(_spinningTimePassed + Time.deltaTime, 0, _barrelsSpinningTime);

        return _currentSpinningSpeed;
    }

    [Command]
    private void CmdSetIsSpinning(bool isSpinning)
    {
        _isSpinning = isSpinning;
    }
}