using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBaseEffects : MonoBehaviour
{
    [SerializeField] private GunBase _gunScript;

    [Header("Shotfire particles")]
    [SerializeField] private ParticleSystem[] _shotfireParticles;
    private bool[] _isShotfirePlaying;

    [Header("Bullet tracer effect")]
    [SerializeField] private GameObject _bulletTracerPrefab;

    [Header("Hit impact")]
    [SerializeField] private GameObject _bulletHitEffectPrefab;

    private int _layer;

    private float _lastShotTime;

    void Start()
    {
        _isShotfirePlaying = new bool[_shotfireParticles.Length];
        _layer = 1 << LayerMask.NameToLayer("Default");
        _gunScript.OnLocalGunShoot += SpawnBulletTracer;
    }

    void Update()
    {
        ShootEffect();

        if (!_gunScript.hasAuthority)
            SimulateShooting();
    }

    private void ShootEffect()
    {
        for (int i = 0; i < _isShotfirePlaying.Length; i++)
        {
            if (_gunScript.IsShooting && !_isShotfirePlaying[i])
            {
                _isShotfirePlaying[i] = true;
                _shotfireParticles[i].Play();
            }
            if (!_gunScript.IsShooting && _isShotfirePlaying[i])
            {
                _isShotfirePlaying[i] = false;
                _shotfireParticles[i].Stop();
            }
        }
    }

    private void SpawnBulletTracer(Ray shotRay, float length)
    {
        Vector3 destination = shotRay.origin + shotRay.direction * length;
        if (Physics.Raycast(shotRay, out RaycastHit hitInfo, length, _layer, QueryTriggerInteraction.Ignore))
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

    private void SimulateShooting()
    {
        if (!_gunScript.IsShooting) return;

        if (Time.time < _lastShotTime + _gunScript.TimeBetweenShots) return;

        foreach (var shotPoint in _gunScript.ShotPoints)
        {
            _gunScript.InitializeShotRay(shotPoint, out Ray shotRay);

            SpawnBulletTracer(shotRay, _gunScript.ShotDistance);
        }
        _lastShotTime = Time.time;
    }
}
