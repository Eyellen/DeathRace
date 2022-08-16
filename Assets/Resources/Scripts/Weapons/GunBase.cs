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

    [field: Header("Gun")]
    [field: SerializeField] public Transform[] ShotPoints { get; private set; }

    [field: Header("Gun settings")]
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public float ShotDistance { get; private set; }
    [field: SerializeField] public float MaxSpreadMagnitude { get; private set; }
    [field: SerializeField] public float TimeBetweenShots { get; private set; }
    [field: SerializeField] public int MaxAmmoSupply { get; private set; }

    private float _lastShotTime;
    private int _currentBulletsCount;

    private int _layer;

    [field: SerializeField]
    [field: SyncVar]
    public bool IsActivated { get; set; }

    [field: SyncVar] public bool IsShooting { get; protected set; }

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
            return Time.time > (_lastShotTime + TimeBetweenShots);
        }
    }
    
    public delegate void GunShootEvent(Ray shotRay, float length);
    public event GunShootEvent OnLocalGunShoot;

    protected virtual void Awake()
    {
        InitializeGunBase();
    }

    protected virtual void Update()
    {
        HandleInput();

#if UNITY_EDITOR || DEBUG_BUILD
        if (_debug)
        {
            foreach (var shotPoint in ShotPoints)
                Debug.DrawRay(shotPoint.position, shotPoint.forward * ShotDistance, Color.red);
        }
#endif
    }

    private void InitializeGunBase()
    {
        _layer = 1 << LayerMask.NameToLayer("Default");
        _currentBulletsCount = MaxAmmoSupply;
    }

    protected virtual void HandleInput()
    {
        if (!hasAuthority) return;

        Shoot(PlayerInput.IsLeftActionPressed);
    }

    protected void SingleShot()
    {
        if (!IsActivated) return;

        if (IsAmmoRunOut)
        {
            CmdSetShooting(false);
            return;
        }
        
        if (!IsTimeBetweenShotsPassed) return;
        _lastShotTime = Time.time;

        _currentBulletsCount--;

        for (int i = 0; i < ShotPoints.Length; i++)
        {
            InitializeShotRay(ShotPoints[i], out Ray shotRay);

            OnLocalGunShoot.Invoke(shotRay, ShotDistance);

#if UNITY_EDITOR || DEBUG_BUILD
            if (_debug)
            {
                Debug.DrawRay(shotRay.origin, shotRay.direction * ShotDistance, Color.green);
            }
#endif

            if (!Physics.Raycast(shotRay, out RaycastHit hitInfo, ShotDistance, _layer, QueryTriggerInteraction.Ignore)) continue;

            IDamageable<int>[] damageables = hitInfo.transform.GetComponents<IDamageable<int>>();
            if (damageables.Length <= 0) continue;

            foreach (IDamageable<int> damageable in damageables)
            {
                damageable.Damage(Damage, hitInfo.collider);
            }
        }
    }

    private void AddSpread(Transform shotPoint, ref Ray shotRay)
    {
        Vector3 xSpread = shotPoint.right * Random.Range(-1f, 1f) * MaxSpreadMagnitude;
        Vector3 ySpread = shotPoint.up * Random.Range(-1f, 1f) * MaxSpreadMagnitude;

        Vector3 spread = xSpread + ySpread;
        spread = spread.magnitude > MaxSpreadMagnitude ? spread.normalized * MaxSpreadMagnitude : spread;

        shotRay.direction += spread / 100;
    }

    public void InitializeShotRay(Transform shotPoint, out Ray shotRay)
    {
        shotRay = new Ray(shotPoint.position, shotPoint.forward);
        AddSpread(shotPoint, ref shotRay);
    }

    protected virtual void Shoot(bool isActionOccurs)
    {
        if (!IsActivated) return;

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
