using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Transform _thisTransform;
    
    [Header("Rocket settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _impactRadius;
    [SerializeField] private float _minDamage;
    [SerializeField] private int _maxDamage;
    [SerializeField] private float _maxTravelDistance;

    private Collider _directHit;
    private Vector3 _startPosition;
    private bool _exploded;

    public delegate void RocketExplodeEvent();
    public event RocketExplodeEvent OnRocketExplode;

    void Start()
    {
        _thisTransform = GetComponent<Transform>();
        _startPosition = transform.position;
    }

    void Update()
    {
        HandleFly();
        HandleTravelLimit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_exploded) return;

        HandleDirectHit(other);
        HandleImpact();
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
        _exploded = true;
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

    private void HandleImpact()
    {
        Collider[] objectsInRadius = Physics.OverlapSphere(_thisTransform.position, _impactRadius);

        foreach (Collider collider in objectsInRadius)
        {
            if (collider == _directHit) continue;
            if (!collider.TryGetComponent(out IDamageable<int> damageable)) continue;

            float distanceToObject = Vector3.Distance(_thisTransform.position, collider.transform.position);
            float coefficient = 1 - distanceToObject / _impactRadius;
            coefficient = Mathf.Clamp(coefficient, 0.2f, 1f);
            damageable.Damage((int)(_maxDamage * coefficient));
        }
    }
}
