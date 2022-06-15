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

    public int Health { get => _health; }

    private void Start()
    {
        _currentCar = gameObject;
        _carCollider = transform.Find("Body/Frame").GetComponent<Collider>();
    }

    public void Damage(int damage, Collider collider)
    {
        if (collider != _carCollider) return;

        if (_health <= 0 && _isDestructed) return;

        CmdSetDamage(damage);

        if (_health > 0) return;

        CmdSetDestructed(true);
        CmdDestruct();
    }

    [Command(requiresAuthority = false)]
    private void CmdSetDamage(int damage)
    {
        _health -= damage;
    }

    [Command(requiresAuthority = false)]
    private void CmdSetDestructed(bool isDestructed)
    {
        _isDestructed = isDestructed;
    }

    [Command(requiresAuthority = false)]
    private void CmdDestruct()
    {
        GameObject destroyedCar = Instantiate(_destroyedCarPrefab, _currentCar.transform.position, _currentCar.transform.rotation);
        InitializeDestroyedCar(destroyedCar);
        NetworkServer.Spawn(destroyedCar);

        NetworkServer.Destroy(_currentCar);
    }

    private void InitializeDestroyedCar(GameObject destroyedCar)
    {
        // Speed inheritance
        destroyedCar.GetComponent<Rigidbody>().velocity = _currentCar.GetComponent<Rigidbody>().velocity;

        destroyedCar.GetComponent<DestroyedCar>().Car = _currentCar;

        // Disabling BackPlate
        GameObject backPlate = _currentCar.transform.Find("Body/BackPlate")?.gameObject;
        backPlate?.SetActive(false);
    }
}