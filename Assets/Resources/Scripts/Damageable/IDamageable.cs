using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable<T>
{
    public T MaxHealth { get; }
    public T CurrentHealth { get; }
    public float HealthRatio { get; }

    public void Damage(T damage, Collider toCollider, Player byPlayer);
    public void Damage01(float coefficient, Collider toCollider, Player byPlayer);
}