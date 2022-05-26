using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedCar : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private GameObject _backPlate;

    [SerializeField] private List<Wheel> _wheels;

    [SerializeField] private float _explosionForce;

    #region Properties
    public Rigidbody Rigidbody { get => _rigidbody; }
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Explode();
    }

    private void Update()
    {
        // Debug
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Explode();
        }
    }

    private void Explode()
    {
        int x = Random.Range(-1, 1);
        int z = Random.Range(-1, 1);
        Vector3 impactPoint = transform.position + new Vector3(x, 0, z);
        _rigidbody.AddForceAtPosition(Vector3.up * _explosionForce, impactPoint, ForceMode.VelocityChange);
    }
}
