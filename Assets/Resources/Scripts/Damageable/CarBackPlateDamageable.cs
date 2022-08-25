using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CarBackPlateDamageable : NetworkBehaviour, IDamageable<int>
{
    private int _maxHealth;

    [SerializeField]
    [SyncVar]
    private int _currentHealth;

    [SyncVar]
    private bool _isBroken;

    private Collider _backPlateCollider;

    [SerializeField] private GameObject _brokenBackPlatePrefab;

    public int MaxHealth => _maxHealth;

    public int CurrentHealth => _currentHealth;

    public float HealthRatio => (float)_currentHealth / _maxHealth;

    [field: SyncVar]
    private Player LastDamagedByPlayer { get; set; }

    [field: Tooltip("Speed boost that will be applied to car after droping BackPlate")]
    [field: SerializeField]
    public float SpeedBoost { get; private set; } = 3f;

    [field: SerializeField]
    public float RecoilForce { get; private set; } = 10f;

    private void Start()
    {
        _maxHealth = _currentHealth;
        _backPlateCollider = transform.Find("Body/BackPlate").GetComponent<Collider>();

        CheckIfBackPlateBroken();
    }

    private void Update()
    {
        if (!hasAuthority) return;

        if (PlayerInput.IsDropBackPlatePressed)
            DropBackPlate();
    }

    public void Damage(int damage, Collider toCollider, Player byPlayer)
    {
        if (toCollider != _backPlateCollider) return;

        if (_currentHealth <= 0 && _isBroken) return;

        CmdSetHealth(_currentHealth -= damage);
        CmdSetDamagedBy(byPlayer);

        if (_currentHealth > 0) return;

        _isBroken = true; // To prevent errors on Client while _isBroken getting synced on Server and Client
        CmdDestructWrapper();
    }

    public void Damage01(float coefficient, Collider toCollider, Player byPlayer)
    {
        Damage((int)(_maxHealth * coefficient), toCollider, byPlayer);
    }

    [Command(requiresAuthority = false)]
    private void CmdSetDamagedBy(Player damagedBy)
    {
        LastDamagedByPlayer = damagedBy;
    }

    [Command(requiresAuthority = false)]
    private void CmdSetHealth(int health)
    {
        _currentHealth = health;
    }

    [Command(requiresAuthority = false)]
    private void CmdSetBroken(bool isBroken)
    {
        _isBroken = isBroken;
    }

    private void CmdDestructWrapper(float recoilForce = 0)
    {
        CmdDestruct(recoilForce);
    }

    [Command(requiresAuthority = false)]
    private void CmdDestruct(float recoilForce)
    {
        _currentHealth = 0;
        _isBroken = true;

        if (_backPlateCollider == null) return;

        GameObject brokenBackPlate = Instantiate(_brokenBackPlatePrefab,
            _backPlateCollider.transform.position, _backPlateCollider.transform.rotation);

        NetworkServer.Spawn(brokenBackPlate);

        RpcDestruct(brokenBackPlate, recoilForce);
        CmdSetBroken(true);
    }

    [ClientRpc]
    private void RpcDestruct(GameObject brokenBackPlate, float recoilForce)
    {
        if (gameObject.TryGetComponent(out CarBase carBase))
        {
            carBase.SpeedLimit += SpeedBoost;
        }

        brokenBackPlate.GetComponent<Spikes>().IgnoreObject = gameObject;
        // Applying recoil force
        brokenBackPlate.GetComponent<Rigidbody>().AddForce(-brokenBackPlate.transform.right * recoilForce, ForceMode.VelocityChange);

        if (_backPlateCollider != null)
            Destroy(_backPlateCollider.gameObject);
    }

    public void Initialize(CarBackPlateDamageable other)
    {
        this._currentHealth = other._currentHealth;
        this._isBroken = other._isBroken;

        CmdSetHealth(other._currentHealth);
        CmdSetBroken(other._isBroken);
    }

    private void CheckIfBackPlateBroken()
    {
        if (!_isBroken) return;

        Destroy(_backPlateCollider.gameObject);
    }

    private void DestroySelf()
    {
        Damage01(1, _backPlateCollider, Player.LocalPlayer);
    }

    private void DropBackPlate()
    {
        CmdDestructWrapper(RecoilForce);
    }
}
