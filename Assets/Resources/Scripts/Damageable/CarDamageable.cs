using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health;

    public void Damage(int damage)
    {
        _health = (_health - damage >= 0) ? _health - damage : 0;

        if (_health <= 0) Die();

        Debug.Log(transform.name + " damaged by " + damage.ToString());
        Debug.Log("current health is " + _health.ToString());
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
