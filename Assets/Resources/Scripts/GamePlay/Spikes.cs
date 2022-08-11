using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float _minSpeedForDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.TryGetComponent(out IDamageable<int> damageable)) return;

        Debug.Log(collision.relativeVelocity.magnitude);

        if (Mathf.Abs(collision.relativeVelocity.magnitude) < _minSpeedForDamage) return;

        float coefficient = (Mathf.Abs(collision.relativeVelocity.magnitude) - _minSpeedForDamage) / (25 - _minSpeedForDamage);
        damageable.Damage01(coefficient, collision.collider);
    }
}
