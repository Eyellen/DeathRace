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
    public bool IsBroken { get => _isBroken; }

    private void Start()
    {
        _backPlateCollider = transform.Find("Body/BackPlate").GetComponent<Collider>();
    }

    public void Damage(int damage, Collider collider)
    {
        if (collider != _backPlateCollider) return;

        if (_health <= 0 && _isBroken) return;

        CmdSetHealth(_health - damage);

        if (_health > 0) return;

        CmdSetBroken(true);
    }

    [Command(requiresAuthority = false)]
    private void CmdSetHealth(int health)
    {
        _health = health;
    }

    [Command(requiresAuthority = false)]
    private void CmdSetBroken(bool isBroken)
    {
        _isBroken = isBroken;
    }

    private void Destruct(bool oldValue, bool newValue)
    {
        if (newValue == false) return;

        _backPlateCollider.transform.parent = null;
        var rigidbody = _backPlateCollider.gameObject.AddComponent<Rigidbody>();
        rigidbody.mass = 100f;
    }

    public void Initialize(CarBackPlateDamageable other)
    {
        this._health = other._health;
        this._isBroken = other._isBroken;
        this._plateMass = other._plateMass;

        CmdSetHealth(other._health);
        CmdSetBroken(other._isBroken);
    }
}
