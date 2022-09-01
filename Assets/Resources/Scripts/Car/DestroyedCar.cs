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
        if (isServer)
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

    [TargetRpc]
    public void TargetOnDestroyedCarSpawned(NetworkConnection target)
    {
        Player.LocalPlayer.GetComponent<CameraManager>().SetThirdPersonCamera(transform);
        Player.LocalPlayer.GetComponent<CameraManager>().SetFreeCamera(seconds: 2);
    }
}
