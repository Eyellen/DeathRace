using System;
using System.Collections;
using UnityEngine;
using Mirror;

public class CarDamageable : NetworkBehaviour, IDamageable<int>
{
    [SerializeField]
    private int _maxHealth;

    [SyncVar]
    private int _health;

    [SyncVar]
    private bool _isDestructed;

    private Collider _carCollider;
    private GameObject _currentCar;
    [SerializeField] private GameObject _destroyedCarPrefab;
    [SerializeField] private GameObject _explosionPrefab;

    public int MaxHealth => _maxHealth;

    public int CurrentHealth => _health;

    public float HealthRatio => (float)_health / _maxHealth;

    [field: SyncVar]
    private Player LastDamagedByPlayer { get; set; }

    /// <summary>
    /// This is event that should be called only on server
    /// </summary>
    public Action<GameObject, Player> OnCarDestroyedByPlayer = (GameObject car, Player byPlayer) =>
    {
        // Assigning kills

        // No need to assign kills if nobody destroyed this car
        if (byPlayer == null) return;

        PlayerSessionStats sessionStats = byPlayer.gameObject.GetComponent<PlayerSessionStats>();

        // Decrement kills score if destroying self
        if (byPlayer == car.GetComponent<CarInfo>().Player)
        {
            sessionStats.CmdSetKills(sessionStats.Kills - 1);
            return;
        }

        sessionStats.CmdSetKills(sessionStats.Kills + 1);
    };

    private void Start()
    {
        CmdSetHealth(_maxHealth);
        _currentCar = gameObject;
        _carCollider = transform.Find("Body/Frame").GetComponent<Collider>();
    }

    [ServerCallback]
    private void OnDestroy()
    {
        SpawnManager.Instance?.RemoveCarFromSpawnedCars(netId);
    }

    public void Damage(int damage, Collider toCollider, Player byPlayer)
    {
        if (toCollider != _carCollider) return;

        if (_health <= 0 && _isDestructed) return;

        CmdSetHealth(_health -= damage);
        CmdSetDamagedBy(byPlayer);

        if (_health > 0) return;

        CmdSetDestructed(_isDestructed = true);
        CmdDestruct();
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
        _health = health;
    }

    [Command(requiresAuthority = false)]
    private void CmdSetDestructed(bool isDestructed)
    {
        _isDestructed = isDestructed;
    }

    [Command(requiresAuthority = false)]
    private void CmdDestruct()
    {
        // Spawning Destroyed Car
        GameObject destroyedCar = Instantiate(_destroyedCarPrefab, _currentCar.transform.position, _currentCar.transform.rotation);
        InitializeDestroyedCar(destroyedCar);
        NetworkServer.Spawn(destroyedCar);

        destroyedCar.GetComponent<DestroyedCar>().TargetOnDestroyedCarSpawned(_currentCar.GetComponent<CarBase>().connectionToClient);

        // Spawning Explosion
        GameObject explosion = Instantiate(_explosionPrefab, _currentCar.transform.position, _currentCar.transform.rotation);
        NetworkServer.Spawn(explosion);

        NetworkServer.Destroy(_currentCar);
        SpawnManager.Instance.TargetOnLocalCarDestroyed(connectionToClient);

        OnCarDestroyedByPlayer?.Invoke(gameObject, LastDamagedByPlayer);
    }

    private void InitializeDestroyedCar(GameObject destroyedCar)
    {
        // Speed inheritance
        destroyedCar.GetComponent<Rigidbody>().velocity = _currentCar.GetComponent<Rigidbody>().velocity;

        destroyedCar.GetComponent<DestroyedCar>().Car = _currentCar;

        if (!destroyedCar.TryGetComponent(out CarBackPlateDamageable backPlateDamageable)) return;
        backPlateDamageable.Initialize(gameObject.GetComponent<CarBackPlateDamageable>());
    }

    public void DestroySelf()
    {
        Damage(_health, _carCollider, Player.LocalPlayer);
    }
}