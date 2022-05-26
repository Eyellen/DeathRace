using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBackPlateDamageable : MonoBehaviour, IDamageable<int>
{
    [SerializeField] private int _health;
    [SerializeField] private float _plateMass;

    public int Health { get => _health; }

    public void Damage(int damage)
    {
        if (_health <= 0) return;

        _health -= damage;

        if (_health > 0) return;

        Destruct();
    }

    private void Destruct()
    {
        transform.parent = null;
        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.mass = 100f;
    }
}
