using UnityEngine;

public interface IDamageable<T>
{
    public void Damage(T damage, Collider collider);
}