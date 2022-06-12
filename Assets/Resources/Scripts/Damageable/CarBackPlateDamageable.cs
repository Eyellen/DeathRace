using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarBackPlateDamageable : NetworkBehaviour, IDamageable<int>
{
    [SyncVar]
    [SerializeField]
    private int _health;

    [SyncVar(hook = nameof(Destruct))]
    private bool _isBroken;

    private Collider _backPlateCollider;

    [SerializeField]
    private float _plateMass;

    public int Health { get => _health; }

    private void Start()
    {
        _backPlateCollider = transform.Find("Body/BackPlate").GetComponent<Collider>();
    }

    public void Damage(int damage, Collider collider)
    {
        if (collider != _backPlateCollider) return;

        if (_health <= 0 && _isBroken) return;

        CmdSetDamage(damage);

        if (_health > 0) return;

        if (_isBroken) return;

        CmdSetBroken(true);
    }

    [Command(requiresAuthority = false)]
    private void CmdSetDamage(int damage)
    {
        _health -= damage;
    }

    [Command(requiresAuthority = false)]
    private void CmdSetBroken(bool isBroken)
    {
        _isBroken = isBroken;
    }

    private void Destruct(bool oldValue, bool newValue)
    {
        _backPlateCollider.transform.parent = null;
        var rigidbody = _backPlateCollider.gameObject.AddComponent<Rigidbody>();
        rigidbody.mass = 100f;
    }
}
