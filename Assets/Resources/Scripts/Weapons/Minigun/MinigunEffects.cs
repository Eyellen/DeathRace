using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Minigun))]
public class MinigunEffects : MonoBehaviour
{
    private Minigun _minigun;

    [Header("Shotfire particles")]
    [SerializeField] private ParticleSystem[] _shorfireParticles;
    private bool _isShotfirEmitting;

    [Header("Bullet cartridge particles")]
    [SerializeField] private GameObject _bulletCartridgeParticlesPrefab;
    [SerializeField] private Transform _cartridgeSpawnPoint;
    [SerializeField] private float _delayBetweenCartridges;
    private float _timeSinceLastCartridgeSpawned;

    [Header("Bullet tracer effect")]
    [SerializeField] private GameObject _bulletTracerPrefab;

    [Header("Hit impact")]
    [SerializeField] private GameObject _hitImpactPrefab;

    void Start()
    {
        _minigun = GetComponent<Minigun>();
        _minigun.OnShoot += SpawnBulletTracer;
    }


    void Update()
    {
        ShootEffect();
        BulletCartridgeEffect();
    }

    private void ShootEffect()
    {
        if(_minigun.IsShooting && !_isShotfirEmitting)
        {
            _isShotfirEmitting = true;
            foreach (var particle in _shorfireParticles)
            {
                var emission = particle.emission;
                emission.enabled = true;
            }
        }

        if(!_minigun.IsShooting && _isShotfirEmitting)
        {
            _isShotfirEmitting = false;
            foreach (var particle in _shorfireParticles)
            {
                var emission = particle.emission;
                emission.enabled = false;
            }
        }
    }

    private void BulletCartridgeEffect()
    {
        if (Time.time <= _timeSinceLastCartridgeSpawned + _delayBetweenCartridges) return;

        if (!_minigun.IsShooting) return;

        _timeSinceLastCartridgeSpawned = Time.time;
        Instantiate(_bulletCartridgeParticlesPrefab, _cartridgeSpawnPoint);
    }

    private void SpawnBulletTracer(Vector3 shotPoint, Vector3 destination)
    {
        if(Physics.Linecast(shotPoint, destination, out RaycastHit hitInfo))
        {
            SpawnHitParticles(hitInfo);

            destination = hitInfo.point;
        }
        GameObject tracer = Instantiate(_bulletTracerPrefab, shotPoint, Quaternion.identity);
        tracer.GetComponent<BulletTracer>().Destination = destination;
    }

    private void SpawnHitParticles(RaycastHit hitInfo)
    {
        var hitParticle = Instantiate(_hitImpactPrefab, hitInfo.point, Quaternion.identity);
        hitParticle.transform.LookAt(hitInfo.point + hitInfo.normal);
    }
}
