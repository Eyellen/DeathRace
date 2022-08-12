using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Minigun : GunBase
{
    [Header("Minigun")]
    [SerializeField] private Transform _minigunBarrels;

    [Header("Minigun settings")]
    [SerializeField] private float _barrelsSpinningTime;
    private float _spinningTimePassed;
    [SyncVar] private bool _isSpinning;
    [SerializeField] private float _maxSpinningSpeed;
    private float _currentSpinningSpeed;

    #region Properties
    public bool IsSpinning { get => _isSpinning; }
    #endregion

    protected override void HandleInput()
    {
        SpinBarrels(PlayerInput.IsLeftActionPressed);
    }

    private void SpinBarrels(bool isActionOccurs)
    {
        if (!IsActivated) return;

        CmdSetShooting(false);

        _currentSpinningSpeed = Mathf.Lerp(0, _maxSpinningSpeed, _spinningTimePassed / _barrelsSpinningTime);
        _minigunBarrels.Rotate(Vector3.forward, _currentSpinningSpeed * Time.deltaTime);
        //Debug.Log(_spinningTimePassed);
        //Debug.Log(_currentSpinningSpeed);

        if (!isActionOccurs)
        {
            CmdSetIsSpinning(false);
            _spinningTimePassed = Mathf.Clamp(_spinningTimePassed - Time.deltaTime, 0, _barrelsSpinningTime);
            return;
        }

        if(!_isSpinning)
        {
            CmdSetIsSpinning(true);
        }
        _spinningTimePassed = Mathf.Clamp(_spinningTimePassed + Time.deltaTime, 0, _barrelsSpinningTime);

        if (_currentSpinningSpeed != _maxSpinningSpeed) return;

        Shoot(isActionOccurs);
    }

    [Command]
    private void CmdSetIsSpinning(bool isSpinning)
    {
        _isSpinning = isSpinning;
    }
}