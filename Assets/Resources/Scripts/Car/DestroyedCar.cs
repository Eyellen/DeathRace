using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyedCar : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    private CarBackPlateDamageable _backPlateDamageable;

    [SyncVar(hook = nameof(DestroyBackPlate))]
    private bool _isBackPlateBroken;

    [SerializeField] private float _explosionForce;

    #region Properties
    public Rigidbody Rigidbody { get => _rigidbody; }
    public GameObject Car { get; set; }
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _backPlateDamageable = GetComponent<CarBackPlateDamageable>();
    }

    void Start()
    {
        if (isServer)
        {
            Explode();
            InitializeBackPlate();
        }

        CheckIfBackPlateBroken();
    }

    private void Explode()
    {
        int x = Random.Range(-1, 1);
        int z = Random.Range(-1, 1);
        Vector3 impactPoint = transform.position + new Vector3(x, 0, z);
        _rigidbody.AddForceAtPosition(Vector3.up * _explosionForce, impactPoint, ForceMode.VelocityChange);
    }

    private void InitializeBackPlate()
    {
#if UNITY_EDITOR
        if (!Car)
        {
            Debug.LogError("Car is not set for DestroyedCar. Can't initialize BackPlate.");
            return;
        }
        if (!Car.GetComponent<CarBackPlateDamageable>())
        {
            Debug.LogError($"Car wasn't set correct or Car doesn't have {nameof(CarBackPlateDamageable)} script. Can't initialize BackPlate");
            return;
        }
#endif
        _backPlateDamageable.Initialize(Car.GetComponent<CarBackPlateDamageable>());
    }

    private void CheckIfBackPlateBroken()
    {
        if(_backPlateDamageable.IsBroken)
        {
            CmdSetIsBackPlateBroken(true);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdSetIsBackPlateBroken(bool isBackPlateBroken)
    {
        _isBackPlateBroken = isBackPlateBroken;
    }

    private void DestroyBackPlate(bool oldValue, bool newValue)
    {
        GameObject currentBackPlate = transform.Find("Body/BackPlate")?.gameObject;

        if (!currentBackPlate) return;

        Destroy(currentBackPlate);
    }
}
