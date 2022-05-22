using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : MonoBehaviour
{
    private Transform _thisTransform;
    private PlayerInput _input;

    [SerializeField] private Transform _minigun;
    [SerializeField] private Transform _shotPoint;

    [Header("Gun settings")]
    [SerializeField] private int _damage;
    [SerializeField] private float _shotDistance;
    [SerializeField] private float _spread;
    [SerializeField] private float _timeBetweenShots;
    private float _lastShotTime;
    [SerializeField] private float _barrelsSpinningTime;
    private float _spinningTimePassed;
    private bool _isSpinning;
    [SerializeField] private float _maxSpinningSpeed;
    private float _currentSpinningSpeed;
    [SerializeField] private int _maxAmmoSupply;
    private int _currentAmmoSupply;

    #region Properties
    public bool IsSpinning { get => _isSpinning; }
    public bool IsShooting { get; private set; }
    #endregion

    private void Awake()
    {
        _thisTransform = GetComponent<Transform>();
        _input = PlayerInput.Instance;

        _currentAmmoSupply = _maxAmmoSupply;
    }

    private void Update()
    {
        SpinBarrels(_input.IsLeftActionPressed);
        Debug.DrawRay(_shotPoint.position, _thisTransform.forward * _shotDistance, Color.red);
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

        Shoot();
    }

    private void Shoot()
    {
        IsShooting = true;
        if (_currentAmmoSupply <= 0)
        {
            IsShooting = false;
            return;
        }

        if (Time.time <= _lastShotTime + _timeBetweenShots) return;
        _lastShotTime = Time.time;
        _currentAmmoSupply--;

        Ray ray = new Ray(_shotPoint.position, _thisTransform.forward);
        ray = AddSpread(ray, _spread);
        Debug.DrawRay(ray.origin, ray.direction * _shotDistance, Color.blue, 0.1f);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, _shotDistance)) return;

        if (!hitInfo.transform.TryGetComponent<IDamageable>(out IDamageable target)) return;

        target.Damage(_damage);
    }

    private Ray AddSpread(Ray ray, float spreadValue)
    {
        Vector3 spread = new Vector3(Random.Range(-1f, 1f) * spreadValue, Random.Range(-1f, 1f) * spreadValue, 0);
        ray.direction += spread / 100;
        return ray;
    }
}