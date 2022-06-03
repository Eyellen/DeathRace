using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Transform _thisTransform;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Rocket settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _maxTravelDistance;
    [SerializeField] private float _impactRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _minDamage;
    [SerializeField] private int _maxDamage;
    
    private Collider _directHit;
    private Vector3 _startPosition;
    private bool _isExploded;

    #region Properties
    public float Speed { get { return _speed; } set { _speed = value; } }
    #endregion

    public delegate void RocketExplodeEvent();
    public event RocketExplodeEvent OnRocketExplode;

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        HandleFly();
        HandleTravelLimit();
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleDirectHit(other);
        Explode();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, _impactRadius);
    }

    private void HandleFly()
    {
        _thisTransform.Translate(_thisTransform.forward * _speed * Time.deltaTime, Space.World);
    }

    private void HandleTravelLimit()
    {
        if (Vector3.Distance(_startPosition, _thisTransform.position) < _maxTravelDistance) return;

        Explode();
    }

    private void Explode()
    {
        if (_isExploded) return;
        _isExploded = true;

        InitializeExplosion();

        OnRocketExplode?.Invoke();
        Destroy(gameObject);
    }

    private void HandleDirectHit(Collider collider)
    {
        if (_directHit) return;

        _directHit = collider;

        if (!collider.TryGetComponent(out IDamageable<int> damageable)) return;

        damageable.Damage(_maxDamage);
    }

    private void InitializeExplosion()
    {
        GameObject explosionPrefab = Instantiate(_explosionPrefab, transform.position, transform.rotation);
        Explosion explosion = explosionPrefab.GetComponent<Explosion>();
        explosion.ExplosionRadius = _impactRadius;
        explosion.ExplosionForce = _explosionForce;
        explosion.MinDamage = _minDamage;
        explosion.MaxDamage = _maxDamage;
        explosion.ExceptionObject = _directHit;
    }
}
