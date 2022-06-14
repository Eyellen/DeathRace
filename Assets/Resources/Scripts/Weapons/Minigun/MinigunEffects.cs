using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunEffects : MonoBehaviour
{
    [SerializeField] private Minigun _minigunScript;
    [SerializeField] private Transform _shotPoint;

    [Header("Shotfire particles")]
    [SerializeField] private ParticleSystem[] _shorfireParticles;
    private bool _isShotfireEmitting;

    [Header("Bullet tracer effect")]
    [SerializeField] private GameObject _bulletTracerPrefab;

    [Header("Hit impact")]
    [SerializeField] private GameObject _bulletHitEffectPrefab;

    void Start()
    {
        _minigunScript.OnGunShoot += SpawnBulletTracer;
    }

    void Update()
    {
        ShootEffect();
    }

    private void ShootEffect()
    {
        if(_minigunScript.IsShooting && !_isShotfireEmitting)
        {
            _isShotfireEmitting = true;
            foreach (var particle in _shorfireParticles)
            {
                var emission = particle.emission;
                emission.enabled = true;
            }
        }

        if(!_minigunScript.IsShooting && _isShotfireEmitting)
        {
            _isShotfireEmitting = false;
            foreach (var particle in _shorfireParticles)
            {
                var emission = particle.emission;
                emission.enabled = false;
            }
        }
    }

    private void SpawnBulletTracer(Vector3 direction, float length)
    {
        Ray shotRay = new Ray(_shotPoint.position, direction.normalized * length);
        Vector3 destination = shotRay.origin + shotRay.direction * length;
        if (Physics.Raycast(shotRay, out RaycastHit hitInfo))
        {
            SpawnHitParticles(hitInfo);

            destination = hitInfo.point;
        }
        GameObject tracer = Instantiate(_bulletTracerPrefab, shotRay.origin, Quaternion.identity);
        tracer.GetComponent<BulletTracer>().Destination = destination;
    }

    private void SpawnHitParticles(RaycastHit hitInfo)
    {
        var hitParticle = Instantiate(_bulletHitEffectPrefab, hitInfo.point, Quaternion.identity);
        hitParticle.transform.LookAt(hitInfo.point + hitInfo.normal);
    }
}
