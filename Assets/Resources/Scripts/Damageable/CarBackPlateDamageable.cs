using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarBackPlateDamageable : NetworkBehaviour, IDamageable<int>
{
    [SerializeField]
    private int _maxHealth;

    [SyncVar]
    private int _health;

    [SyncVar]
    private bool _isBroken;

    private Collider _backPlateCollider;

    [SerializeField] private GameObject _brokenBackPlatePrefab;

    public int Health { get => _health; }
    public bool IsBroken { get => _isBroken; }

    [field: Tooltip("Speed boost that will be applied to car after droping BackPlate")]
    [field: SerializeField]
    public float SpeedBoost { get; private set; } = 3f;

    private void Start()
    {
        CmdSetHealth(_maxHealth);
        _backPlateCollider = transform.Find("Body/BackPlate").GetComponent<Collider>();

        CheckIfBackPlateBroken();
    }

    private void Update()
    {
        if (PlayerInput.IsDropBackPlatePressed)
            DropBackPlate();
    }

    private void OnDestroy()
    {
        gameObject.GetComponent<CarBase>().SpeedLimit += SpeedBoost;
    }

    public void Damage(int damage, Collider collider)
    {
        if (collider != _backPlateCollider) return;

        if (_health <= 0 && _isBroken) return;

        CmdSetHealth(_health -= damage);

        if (_health > 0) return;

        _isBroken = true; // To prevent errors on Client while _isBroken getting synced on Server and Client
        CmdDestruct();
    }

    public void Damage01(float coefficient, Collider collider)
    {
        Damage((int)(_maxHealth * coefficient), collider);
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
        if (_backPlateCollider == null) return;

        GameObject brokenBackPlate = Instantiate(_brokenBackPlatePrefab,
            _backPlateCollider.transform.position, _backPlateCollider.transform.rotation);

        RpcDestruct();

        GameObject ownerPlayer = gameObject.GetComponent<CarInfo>().Player.gameObject;
        NetworkServer.Spawn(brokenBackPlate, ownerPlayer);

        CmdSetBroken(true);
    }

    [ClientRpc]
    private void RpcDestruct()
    {
        if (_backPlateCollider != null)
            Destroy(_backPlateCollider.gameObject);
    }

    public void Initialize(CarBackPlateDamageable other)
    {
        this._health = other._health;
        this._isBroken = other._isBroken;

        CmdSetHealth(other._health);
        CmdSetBroken(other._isBroken);
    }

    private void CheckIfBackPlateBroken()
    {
        if (!_isBroken) return;

        Destroy(_backPlateCollider.gameObject);
    }

    private void DestroySelf()
    {
        Damage01(1, _backPlateCollider);
    }

    private void DropBackPlate()
    {
        DestroySelf();
    }
}
