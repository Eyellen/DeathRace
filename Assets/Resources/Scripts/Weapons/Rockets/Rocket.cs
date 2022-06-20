using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Transform _thisTransform;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Rocket settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _maxTravelTime;
    [SerializeField] private float _impactRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _minDamage;
    [SerializeField] private int _maxDamage;

    private Collider _directHit;
    private bool _isExploded;

    #region Properties
    public float Speed { get { return _speed; } set { _speed = value; } }
    #endregion

    public delegate void RocketExplodeEvent();
    public event RocketExplodeEvent OnRocketExplode;

#if UNITY_EDITOR || DEBUG_BUILD
    [Header("Debugging")]
    [SerializeField] private bool _debug;
#endif

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();

        StartCoroutine(HandleTravelLimit(_maxTravelTime));
    }

    private void FixedUpdate()
    {
        HandleFly();
        CheckHit();
    }

    private void CheckHit()
    {
        Ray direction = new Ray(_thisTransform.position, _thisTransform.forward);
        if (!Physics.SphereCast(direction, 0.03f, out RaycastHit hitInfo, _speed * Time.deltaTime)) return;

#if UNITY_EDITOR || DEBUG_BUILD
        if (_debug)
        {
            Debug.Log("Rocket hitted " + hitInfo.transform.name);
        }
#endif

        HandleDirectHit(hitInfo);
        Explode();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, _impactRadius);
    }

    private void HandleFly()
    {
        _thisTransform.Translate(_thisTransform.forward * _speed * Time.deltaTime, Space.World);
    }

    private IEnumerator HandleTravelLimit(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Explode();
    }

    private void Explode()
    {
        if (_isExploded) return;
        _isExploded = true;

        InitializeExplosion();

        OnRocketExplode?.Invoke();
        Destroy(gameObject);
    }

    private void HandleDirectHit(RaycastHit hitInfo)
    {
        if (_directHit) return;

        _directHit = hitInfo.collider;

        IDamageable<int>[] damageables = hitInfo.transform.GetComponents<IDamageable<int>>();
        if (damageables.Length <= 0) return;

        foreach (IDamageable<int> damageable in damageables)
        {
            damageable.Damage(_maxDamage, hitInfo.collider);
        }

        //if (!hitInfo.transform.TryGetComponent(out IDamageable<int> damageable)) return;

        //damageable.Damage(_maxDamage, hitInfo.collider);
    }

    private void InitializeExplosion()
    {
        GameObject explosionPrefab = Instantiate(_explosionPrefab, transform.position, transform.rotation);
        Explosion explosion = explosionPrefab.GetComponent<Explosion>();
        explosion.ExplosionRadius = _impactRadius;
        explosion.ExplosionForce = _explosionForce;
        explosion.MinDamage = _minDamage;
        explosion.MaxDamage = _maxDamage;
        explosion.ExceptionObjectCollider = _directHit;
    }
}
