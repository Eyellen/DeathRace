using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGun : MonoBehaviour
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
    [SerializeField] private float _barrelsSpinningTime;
    [SerializeField] private int _maxAmmoSupply;

    private float _lastShotTime;
    private int _currentAmmoSupply;

    private float _spinningStartTime;
    private bool _isSpinning;

    private float _currentSpinningSpeed;
    private float _currentSpinningSpeedTarget;
    [SerializeField] private float _maxSpinningSpeed;

    [SerializeField] private bool RotateBarrels;

    private void Awake()
    {
        _thisTransform = GetComponent<Transform>();
        _input = PlayerInput.Instance;
    }

    private void Update()
    {
        //BarrelsSpinning(_input.IsLeftActionPressed);
        SpinBarrels(RotateBarrels);
        Debug.DrawRay(_shotPoint.position, _thisTransform.forward * _shotDistance, Color.red);
    }

    private void SpinBarrels(bool isActionOccurs)
    {
        _currentSpinningSpeed = Mathf.Lerp(_currentSpinningSpeed, _currentSpinningSpeedTarget, 0.5f * Time.deltaTime);
        _minigun.Rotate(Vector3.forward, _currentSpinningSpeed * Time.deltaTime);

        if (!isActionOccurs)
        {
            _isSpinning = false;
            _currentSpinningSpeedTarget = 0;
            return;
        }

        if(!_isSpinning)
        {
            _isSpinning = true;
            _spinningStartTime = Time.time;
            _currentSpinningSpeedTarget = _maxSpinningSpeed;
        }

        if (Time.time <= _spinningStartTime + _barrelsSpinningTime) return;

        Shoot();
    }

    private void Shoot()
    {
        if (Time.time <= _lastShotTime + _timeBetweenShots) return;
        _lastShotTime = Time.time;

        Vector3 shootSpread = new Vector3(Random.Range(-1f, 1f) * _spread, Random.Range(-1f, 1f) * _spread, 0);
        Ray ray = new Ray(_shotPoint.position, _thisTransform.forward + shootSpread / 100);
        Debug.DrawRay(ray.origin, ray.direction * _shotDistance, Color.blue, 0.1f);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, _shotDistance)) return;

        Debug.Log(hitInfo.transform.name);//
        if (!hitInfo.transform.TryGetComponent<IDamageable>(out IDamageable target)) return;

        target.Damage(_damage);
    }
}