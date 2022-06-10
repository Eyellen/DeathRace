using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBackPlateDamageable : MonoBehaviour, IDamageable<int>
{
    [SerializeField] private int _health;
    private Collider _backPlateCollider;
    [SerializeField] private float _plateMass;

    public int Health { get => _health; }

    private void Start()
    {
        _backPlateCollider = transform.Find("Body/BackPlate").GetComponent<Collider>();
    }

    public void Damage(int damage, Collider collider)
    {
        if (collider != _backPlateCollider) return;

        if (_health <= 0) return;

        _health -= damage;

        if (_health > 0) return;

        Destruct();
    }

    private void Destruct()
    {
        _backPlateCollider.transform.parent = null;
        var rigidbody = _backPlateCollider.gameObject.AddComponent<Rigidbody>();
        rigidbody.mass = 100f;
    }
}
