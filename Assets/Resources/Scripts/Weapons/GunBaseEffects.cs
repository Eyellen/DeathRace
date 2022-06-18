using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBaseEffects : MonoBehaviour
{
    [SerializeField] private GunBase _gunScript;
    [SerializeField] private Transform _shotPoint;

    [Header("Shotfire particles")]
    [SerializeField] private ParticleSystem _shotfireParticles;
    private bool _isShotfirePlaying;

    [Header("Bullet tracer effect")]
    [SerializeField] private GameObject _bulletTracerPrefab;

    [Header("Hit impact")]
    [SerializeField] private GameObject _bulletHitEffectPrefab;

    void Start()
    {
        _gunScript.OnGunShoot += SpawnBulletTracer;
    }

    void Update()
    {
        ShootEffect();
    }

    private void ShootEffect()
    {
        if (_gunScript.IsShooting && !_isShotfirePlaying)
        {
            _isShotfirePlaying = true;
            _shotfireParticles.Play();
        }
        if (!_gunScript.IsShooting && _isShotfirePlaying)
        {
            _isShotfirePlaying = false;
            _shotfireParticles.Stop();
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
