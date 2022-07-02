using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GunBase : NetworkBehaviour
{
#if UNITY_EDITOR || DEBUG_BUILD
    [Header("Debugging")]
    [SerializeField] private bool _debug;
#endif

    [Header("Gun")]
    [SerializeField] private Transform _shotPoint;

    [Header("Gun settings")]
    [SerializeField] private int _damage;
    [SerializeField] private float _shotDistance;
    [SerializeField] private float _maxSpreadMagnitude;
    [SerializeField] private float _timeBetweenShots;
    [SerializeField] private int _maxAmmoSupply;

    private float _lastShotTime;
    private int _currentBulletsCount;

    #region Properties
    private bool IsAmmoRunOut
    {
        get
        {
            return _currentBulletsCount <= 0;
        }
    }
    private bool IsTimeBetweenShotsPassed
    {
        get
        {
            return Time.time > (_lastShotTime + _timeBetweenShots);
        }
    }
    [field: SyncVar] public bool IsShooting { get; protected set; }
    #endregion

    public delegate void GunShootEvent(Vector3 direction, float length);
    public event GunShootEvent OnGunShoot;

    [SyncVar(hook = nameof(HookOnGunShoot))] 
    private Vector3 _gunShootEventDirection;

    protected virtual void Awake()
    {
        InitializeGunBase();
    }

    protected virtual void Update()
    {
        if (!isLocalPlayer) return;

        HandleInput();

#if UNITY_EDITOR || DEBUG_BUILD
        if (_debug)
        {
            Debug.DrawRay(_shotPoint.position, _shotPoint.forward * _shotDistance, Color.red);
        }
#endif
    }

    private void InitializeGunBase()
    {
        _currentBulletsCount = _maxAmmoSupply;
    }

    protected virtual void HandleInput()
    {
        Shoot(PlayerInput.IsLeftActionPressed);
    }

    protected void SingleShot()
    {
        if (IsAmmoRunOut)
        {
            CmdSetShooting(false);
            return;
        }
        
        if (!IsTimeBetweenShotsPassed) return;
        _lastShotTime = Time.time;

        _currentBulletsCount--;

        InitializeShotRay(out Ray shotRay);
        //OnGunShoot?.Invoke(shotRay.direction, _shotDistance);
        CmdSetGunShootEventDirection(shotRay.direction);

#if UNITY_EDITOR || DEBUG_BUILD
        if (_debug)
        {
            Debug.DrawRay(shotRay.origin, shotRay.direction * _shotDistance, Color.green, 0.5f);
        }
#endif

        if (!Physics.Raycast(shotRay, out RaycastHit hitInfo, _shotDistance)) return;

        IDamageable<int>[] damageables = hitInfo.transform.GetComponents<IDamageable<int>>();
        if (damageables.Length <= 0) return;

        foreach (IDamageable<int> damageable in damageables)
        {
            damageable.Damage(_damage, hitInfo.collider);
        }

        //if (!hitInfo.transform.TryGetComponent(out IDamageable<int> damageable)) return;

        //damageable.Damage(_damage, hitInfo.collider);
    }

    [Command(requiresAuthority = false)]
    private void CmdSetGunShootEventDirection(Vector3 direction)
    {
        _gunShootEventDirection = direction;
    }

    private void HookOnGunShoot(Vector3 oldValue, Vector3 newValue)
    {
        OnGunShoot?.Invoke(newValue, _shotDistance);
    }

    private void AddSpread(ref Ray shotRay)
    {
        Vector3 xSpread = _shotPoint.right * Random.Range(-1f, 1f) * _maxSpreadMagnitude;
        Vector3 ySpread = _shotPoint.up * Random.Range(-1f, 1f) * _maxSpreadMagnitude;

        Vector3 spread = xSpread + ySpread;
        spread = spread.magnitude > _maxSpreadMagnitude ? spread.normalized * _maxSpreadMagnitude : spread;

        shotRay.direction += spread / 100;
    }

    private void InitializeShotRay(out Ray shotRay)
    {
        shotRay = new Ray(_shotPoint.position, _shotPoint.forward);
        AddSpread(ref shotRay);
    }

    protected virtual void Shoot(bool isActionOccurs)
    {
        CmdSetShooting(isActionOccurs);
        if(isActionOccurs)
        {
            SingleShot();
        }
    }

    [Command(requiresAuthority = false)]
    protected void CmdSetShooting(bool isShooting)
    {
        IsShooting = isShooting;
    }
}
