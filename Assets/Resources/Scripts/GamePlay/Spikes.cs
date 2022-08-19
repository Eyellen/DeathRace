using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private float _minSpeedForDamage;
    [SerializeField] private float _damageCoefficientMultiplier = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (Mathf.Abs(collision.relativeVelocity.magnitude) < _minSpeedForDamage) return;

        // To prevent caling on server
        // Need to be called only via Command
        if (!collision.transform.root.TryGetComponent(out CarBase carBase)) return;
        if (!carBase.hasAuthority) return;

#if UNITY_EDITOR
        Debug.Log(collision.relativeVelocity.magnitude);
#endif

        IDamageable<int>[] damageables = collision.gameObject.GetComponents<IDamageable<int>>();
        if (damageables.Length == 0) return;

        float coefficient = (Mathf.Abs(collision.relativeVelocity.magnitude) - _minSpeedForDamage) / (25 - _minSpeedForDamage) * _damageCoefficientMultiplier;
        foreach (var damageable in damageables)
        {
            damageable.Damage01(coefficient, collision.collider);
        }
    }
}
