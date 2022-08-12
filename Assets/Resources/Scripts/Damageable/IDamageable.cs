using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable<T>
{
    public void Damage(T damage, Collider collider);
    public void Damage01(float coefficient, Collider collider);
}