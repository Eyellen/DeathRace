using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyedCar : NetworkBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] private float _explosionForce;

    #region Properties
    public Rigidbody Rigidbody { get => _rigidbody; }
    public GameObject Car { get; set; }
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
        if (isClient) return;

#if UNITY_EDITOR
        if (!Car)
        {
            Debug.LogError("Car is not set for DestroyedCar. Can't check if back plate is broken." +
                $"\n{nameof(DestroyedCar)}.{nameof(CheckIfBackPlateBroken)}()");
            return;
        }
#endif

        // Looking for Car's BackPlate
        Transform backPlate = Car.transform.Find("Body/BackPlate");

        // Destroying DestroyedCar's BackPlate if Car's BackPlate has been destroyed
        if(!backPlate)
        {
            Transform currentBackPlate = transform.Find("Body/BackPlate");
            Destroy(currentBackPlate.gameObject);
        }
    }
}
