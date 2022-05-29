using UnityEngine;

public class CarDamageable : MonoBehaviour, IDamageable<int>
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _currentCar;
    [SerializeField] private GameObject _destroyedCarPrefab;

    public int Health { get => _health; }

    public void Damage(int damage)
    {
        if (_health <= 0) return;

        _health -= damage;

        if (_health > 0) return;

        Destruct();
    }

    private void Destruct()
    {
        var destroyedCar = Instantiate(_destroyedCarPrefab, transform.position, transform.rotation);
        destroyedCar.GetComponent<Rigidbody>().velocity = _currentCar.GetComponent<Rigidbody>().velocity;
        Destroy(_currentCar);
    }
}
