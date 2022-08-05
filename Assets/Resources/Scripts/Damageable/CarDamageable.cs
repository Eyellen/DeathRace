using System;
using System.Collections;
using UnityEngine;
using Mirror;

public class CarDamageable : NetworkBehaviour, IDamageable<int>
{
    [SyncVar]
    [SerializeField]
    private int _health;

    [SyncVar]
    private bool _isDestructed;

    private Collider _carCollider;
    private GameObject _currentCar;
    [SerializeField] private GameObject _destroyedCarPrefab;
    [SerializeField] private GameObject _explosionPrefab;

    public int Health { get => _health; }

    public event Action OnCarDestroyed = () => Debug.Log("OnCarDestroyedCalled");

    private void Start()
    {
        _currentCar = gameObject;
        _carCollider = transform.Find("Body/Frame").GetComponent<Collider>();
    }

    [ServerCallback]
    private void OnDestroy()
    {
        SpawnManager.Instance?.RemoveCarFromSpawnedCars(netId);
    }

    public void Damage(int damage, Collider collider)
    {
        if (collider != _carCollider) return;

        if (_health <= 0 && _isDestructed) return;

        CmdSetHealth(_health -= damage);

        if (_health > 0) return;

        CmdSetDestructed(_isDestructed = true);
        CmdDestruct();
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
        TargetCallOnCarDestroyed(_currentCar.GetComponent<CarBase>().connectionToClient);

        // Spawning Destroyed Car
        GameObject destroyedCar = Instantiate(_destroyedCarPrefab, _currentCar.transform.position, _currentCar.transform.rotation);
        InitializeDestroyedCar(destroyedCar);
        NetworkServer.Spawn(destroyedCar);

        destroyedCar.GetComponent<DestroyedCar>().TargetOnDestroyedCarSpawned(_currentCar.GetComponent<CarBase>().connectionToClient);

        // Spawning Explosion
        GameObject explosion = Instantiate(_explosionPrefab, _currentCar.transform.position, _currentCar.transform.rotation);
        NetworkServer.Spawn(explosion);

        NetworkServer.Destroy(_currentCar);
    }

    [TargetRpc]
    private void TargetCallOnCarDestroyed(NetworkConnection target)
    {
        OnCarDestroyed?.Invoke();
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
        Damage(_health, _carCollider);
    }
}