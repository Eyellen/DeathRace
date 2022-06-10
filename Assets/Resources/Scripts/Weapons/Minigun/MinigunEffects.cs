using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunEffects : MonoBehaviour
{
    [SerializeField] private Minigun _minigun;

    [Header("Shotfire particles")]
    [SerializeField] private ParticleSystem[] _shorfireParticles;
    private bool _isShotfirEmitting;

    [Header("Bullet tracer effect")]
    [SerializeField] private GameObject _bulletTracerPrefab;

    [Header("Hit impact")]
    [SerializeField] private GameObject _hitImpactPrefab;

    void Start()
    {
        _minigun.OnGunShoot += SpawnBulletTracer;
    }

    void Update()
    {
        ShootEffect();
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

    private void SpawnBulletTracer(Ray shotRay, float length)
    {
        Vector3 destination = shotRay.origin + shotRay.direction * length;
        if(Physics.Raycast(shotRay, out RaycastHit hitInfo))
        {
            SpawnHitParticles(hitInfo);

            destination = hitInfo.point;
        }
        GameObject tracer = Instantiate(_bulletTracerPrefab, shotRay.origin, Quaternion.identity);
        tracer.GetComponent<BulletTracer>().Destination = destination;
    }

    private void SpawnHitParticles(RaycastHit hitInfo)
    {
        var hitParticle = Instantiate(_hitImpactPrefab, hitInfo.point, Quaternion.identity);
        hitParticle.transform.LookAt(hitInfo.point + hitInfo.normal);
    }
}
