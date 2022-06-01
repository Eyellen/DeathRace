using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedCar : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private List<Wheel> _wheels;

    [SerializeField] private float _explosionForce;

    #region Properties
    public Rigidbody Rigidbody { get => _rigidbody; }
    public GameObject CarFrame { get; set; }
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        CheckIfBackPlateBroken();
        Explode();
    }

    private void Explode()
    {
        int x = Random.Range(-1, 1);
        int z = Random.Range(-1, 1);
        Vector3 impactPoint = transform.position + new Vector3(x, 0, z);
        _rigidbody.AddForceAtPosition(Vector3.up * _explosionForce, impactPoint, ForceMode.VelocityChange);
    }

    private void CheckIfBackPlateBroken()
    {
        // Looking for Car's origin transform
        Transform car = CarFrame.transform;
        while (car.parent)
        {
            car = car.parent;
        }

        // Looking for Car's BackPlate
        Transform backPlate = car.Find("Body/BackPlate");

        // Destroying DestroyedCar's BackPlate if Car's BackPlate has been destroyed
        if(!backPlate)
        {
            Transform currentBackPlate = transform.Find("Body/BackPlate");
            Destroy(currentBackPlate.gameObject);
        }
    }
}
