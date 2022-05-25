using UnityEngine;

public class CarDamageable : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _destroyedCar;

    public void Damage(int damage)
    {
        if (_health <= 0) return;

        _health -= damage;

        if (_health > 0) return;

        Destruct();
    }

    private void Destruct()
    {
        var destroyedCar = Instantiate(_destroyedCar, transform.position, transform.rotation);
        destroyedCar.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity;
        Destroy(gameObject);
    }
}
