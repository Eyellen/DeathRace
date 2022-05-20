using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    protected Transform _thisTransform;
    protected PlayerInput _input;
    [SerializeField] protected Transform _shotPoint;

    [Header("Gun settings")]
    [SerializeField] protected int _damage;
    [SerializeField] protected float _shotDistance;
    [SerializeField] protected float _spread;
    [SerializeField] protected float _timeBetweenShots;
    [SerializeField] protected float _reloadTime;
    [SerializeField] protected int _magazineSize;

    protected AudioSource _audioSource;
    [SerializeField] protected AudioClip _shootSound;
    [SerializeField] protected AudioClip _reloadSound;

    protected int _currentBulletsCount;
    protected float _lastShotTime;

    private void Awake()
    {
        _thisTransform = GetComponent<Transform>();
        _input = PlayerInput.Instance;
        _audioSource = GetComponent<AudioSource>();
    }

    protected void Start()
    {
        
    }

    protected void Update()
    {
        if(_input.IsLeftActionPressed)
        {
            Shoot();
        }
        Debug.DrawRay(_shotPoint.position, _thisTransform.forward * _shotDistance, Color.red);
    }

    protected void Shoot()
    {
        if (Time.time <= _lastShotTime + _timeBetweenShots) return;
        _lastShotTime = Time.time;

        _audioSource.clip = _shootSound;
        _audioSource.Play();

        Vector3 shootSpread = new Vector3(Random.Range(-1f, 1f) * _spread, Random.Range(-1f, 1f) * _spread, 0);
        Ray ray = new Ray(_shotPoint.position, _thisTransform.forward + shootSpread / 100);
        Debug.DrawRay(ray.origin, ray.direction * _shotDistance, Color.blue, 0.1f);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, _shotDistance)) return;

        Debug.Log(hitInfo.transform.name);//
        if (!hitInfo.transform.TryGetComponent<IDamageable>(out IDamageable target)) return;

        target.Damage(_damage);
    }
}
