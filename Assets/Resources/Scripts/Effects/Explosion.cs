using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Explosion : NetworkBehaviour
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _minDamage;
    [SerializeField] private float _maxDamage;

    #region Properties
    public Player CausedByPlayer { get; set; }
    public float ExplosionRadius { get { return _explosionRadius; } set { _explosionRadius = value; } }
    public float ExplosionForce { get { return _explosionForce; } set { _explosionForce = value; } }
    public float MinDamage { get { return _minDamage; } set { _minDamage = value; } }
    public float MaxDamage { get { return _maxDamage; } set { _maxDamage = value; } }
    [field: SerializeField] public Collider ExceptionObjectCollider { get; set; }
    #endregion

    private void Start()
    {
        HandleExplosion();
    }

    private void HandleExplosion()
    {
        Collider[] objectsInRadius = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (Collider collider in objectsInRadius)
        {
            ApplyExplosionForce(collider);
            ApplyDamage(collider);
        }
    }

    private void ApplyExplosionForce(Collider collider)
    {
        if (!collider.TryGetComponent(out Rigidbody rigidbody)) return;

        rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
    }

    [ServerCallback]
    private void ApplyDamage(Collider collider)
    {
        if (collider == ExceptionObjectCollider) return;
        //if (!collider.transform.root.TryGetComponent(out IDamageable<int> damageable)) return;

        IDamageable<int>[] damageables = collider.transform.root.GetComponents<IDamageable<int>>();
        if (damageables.Length <= 0) return;

        float distanceToObject = Vector3.Distance(transform.position, collider.transform.position);
        float coefficient = 1 - distanceToObject / _explosionRadius;
        coefficient = Mathf.Clamp(coefficient, 0.2f, 1f);

        foreach (IDamageable<int> damageable in damageables)
        {
            damageable.Damage((int)(_maxDamage * coefficient), collider, CausedByPlayer);
        }
    }
}