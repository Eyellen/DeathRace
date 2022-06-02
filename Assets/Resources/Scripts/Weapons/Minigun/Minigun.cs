using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : GunBase
{
    [SerializeField] private Transform _minigun;

    [Header("Minigun settings")]
    [SerializeField] private float _barrelsSpinningTime;
    private float _spinningTimePassed;
    private bool _isSpinning;
    [SerializeField] private float _maxSpinningSpeed;
    private float _currentSpinningSpeed;

    #region Properties
    public bool IsSpinning { get => _isSpinning; }
    #endregion

    protected override void HandleInput()
    {
        SpinBarrels(_input.IsLeftActionPressed);
    }

    private void SpinBarrels(bool isActionOccurs)
    {
        IsShooting = false;

        _currentSpinningSpeed = Mathf.Lerp(0, _maxSpinningSpeed, _spinningTimePassed / _barrelsSpinningTime);
        _minigun.Rotate(Vector3.forward, _currentSpinningSpeed * Time.deltaTime);
        //Debug.Log(_spinningTimePassed);
        //Debug.Log(_currentSpinningSpeed);

        if (!isActionOccurs)
        {
            _isSpinning = false;
            _spinningTimePassed = Mathf.Clamp(_spinningTimePassed - Time.deltaTime, 0, _barrelsSpinningTime);
            return;
        }

        if(!_isSpinning)
        {
            _isSpinning = true;
        }
        _spinningTimePassed = Mathf.Clamp(_spinningTimePassed + Time.deltaTime, 0, _barrelsSpinningTime);

        if (_currentSpinningSpeed != _maxSpinningSpeed) return;

        Shoot(isActionOccurs);
    }
}