using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarBackPlateDamageable : NetworkBehaviour, IDamageable<int>
{
    [SyncVar]
    [SerializeField]
    private int _health;

    [SyncVar]
    private bool _isBroken;

    private Collider _backPlateCollider;

    [SerializeField]
    private float _plateMass;

    public int Health { get => _health; }
    public bool IsBroken { get => _isBroken; }

    private void Start()
    {
        _backPlateCollider = transform.Find("Body/BackPlate").GetComponent<Collider>();
        
        CheckIfBackPlateBroken();
    }

    public void Damage(int damage, Collider collider)
    {
        if (collider != _backPlateCollider) return;

        if (_health <= 0 && _isBroken) return;

        CmdSetHealth(_health -= damage);

        if (_health > 0) return;

        _isBroken = true; // To prevent errors on Client while _isBroken getting synced on Server and other Clients
        CmdSetBroken(true);
        CmdDestruct();
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

    [Command(requiresAuthority = false)]
    private void CmdDestruct()
    {
        Debug.Log("CmdDestruct has been called");
        RpcDestruct();
    }

    [ClientRpc]
    private void RpcDestruct()
    {
        Debug.Log("RpcDestruct has been called");
        Destruct();
    }

    private void Destruct()
    {
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

    [ClientCallback]
    private void CheckIfBackPlateBroken()
    {
        if (!_isBroken) return;

        Destroy(_backPlateCollider.gameObject);
    }
}
