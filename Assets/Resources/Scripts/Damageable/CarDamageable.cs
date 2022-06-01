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
        GameObject destroyedCar = Instantiate(_destroyedCarPrefab, _currentCar.transform.position, _currentCar.transform.rotation);
        InitializeDestroyedCar(destroyedCar);
        Destroy(_currentCar);
    }

    private void InitializeDestroyedCar(GameObject destroyedCar)
    {
        // Speed inheritance
        destroyedCar.GetComponent<Rigidbody>().velocity = _currentCar.GetComponent<Rigidbody>().velocity;

        destroyedCar.GetComponent<DestroyedCar>().CarFrame = gameObject;

        // Disabling BackPlate
        GameObject backPlate = _currentCar.transform.Find("Body/BackPlate")?.gameObject;
        backPlate?.SetActive(false);
    }
}
