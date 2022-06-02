using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    protected PlayerInput _input;
    [SerializeField] private Transform _shotPoint;

    [Header("Gun settings")]
    [SerializeField] private int _damage;
    [SerializeField] private float _shotDistance;
    [SerializeField] private float _maxSpreadMagnitude;
    [SerializeField] private float _timeBetweenShots;
    [SerializeField] private int _maxAmmoSupply;

    private float _lastShotTime;
    private int _currentBulletsCount;

    #region Properties
    protected bool IsAmmoRunOut
    {
        get
        {
            return _currentBulletsCount <= 0;
        }
    }
    protected bool IsTimeBetweenShotsPassed
    {
        get
        {
            return Time.time > (_lastShotTime + _timeBetweenShots);
        }
    }
    #endregion

    public delegate void GunShootEvent();
    public event GunShootEvent OnGunShoot;

    protected virtual void Awake()
    {
        InitializeGunBase();
    }

    protected virtual void Update()
    {
        HandleInput();
        Debug.DrawRay(_shotPoint.position, _shotPoint.forward * _shotDistance, Color.red);
    }

    private void InitializeGunBase()
    {
        _input = PlayerInput.Instance;
        _currentBulletsCount = _maxAmmoSupply;
    }

    protected virtual void HandleInput()
    {
        if (_input.IsLeftActionPressed)
        {
            Shoot();
        }
    }

    protected void SingleShot()
    {
        if (IsAmmoRunOut) return;

        if (!IsTimeBetweenShotsPassed) return;
        _lastShotTime = Time.time;

        _currentBulletsCount--;
        OnGunShoot?.Invoke();

        InitializeShotRay(out Ray shotRay);
        Debug.DrawRay(shotRay.origin, shotRay.direction * _shotDistance, Color.blue, 0.5f);
        if (!Physics.Raycast(shotRay, out RaycastHit hitInfo, _shotDistance)) return;

        if (!hitInfo.collider.TryGetComponent(out IDamageable<int> damageable)) return;

        damageable.Damage(_damage);
    }

    private void AddSpread(ref Ray shotRay)
    {
        Vector3 xSpread = _shotPoint.right * Random.Range(-1f, 1f) * _maxSpreadMagnitude;
        Vector3 ySpread = _shotPoint.up * Random.Range(-1f, 1f) * _maxSpreadMagnitude;

        Vector3 spread = xSpread + ySpread;
        spread = spread.magnitude > _maxSpreadMagnitude ? spread.normalized * _maxSpreadMagnitude : spread;

        shotRay.direction += spread / 100;
    }

    private void InitializeShotRay(out Ray shotRay)
    {
        shotRay = new Ray(_shotPoint.position, _shotPoint.forward);
        AddSpread(ref shotRay);
    }

    protected virtual void Shoot()
    {
        SingleShot();
    }
}
