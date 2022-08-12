using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float _minSpeedForDamage;

    private void OnCollisionEnter(Collision collision)
    {
        if (Mathf.Abs(collision.relativeVelocity.magnitude) < _minSpeedForDamage) return;

        IDamageable<int>[] damageables = collision.gameObject.GetComponents<IDamageable<int>>();
        if (damageables.Length == 0) return;

        float coefficient = (Mathf.Abs(collision.relativeVelocity.magnitude) - _minSpeedForDamage) / (25 - _minSpeedForDamage);
        foreach (var damageable in damageables)
        {
            damageable.Damage01(coefficient, collision.collider);
        }

        //if (!collision.gameObject.TryGetComponent(out IDamageable<int> damageable)) return;

        //Debug.Log(collision.relativeVelocity.magnitude);

        //if (Mathf.Abs(collision.relativeVelocity.magnitude) < _minSpeedForDamage) return;

        //float coefficient = (Mathf.Abs(collision.relativeVelocity.magnitude) - _minSpeedForDamage) / (25 - _minSpeedForDamage);
        //damageable.Damage01(coefficient, collision.collider);
    }
}
