using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{
    [SerializeField] private GunBase _gun;

    [Header("Bullet cartridge particles")]
    [SerializeField] private ParticleSystem _bulletCartridgeParticles;
    private bool _isBulletCartridgeParticlesPlaying;

    void Update()
    {
        BulletCartridgeEffect();
    }

    private void BulletCartridgeEffect()
    {
        if(_gun.IsShooting && !_isBulletCartridgeParticlesPlaying)
        {
            _isBulletCartridgeParticlesPlaying = true;
            _bulletCartridgeParticles.Play();
        }
        if (!_gun.IsShooting && _isBulletCartridgeParticlesPlaying)
        {
            _isBulletCartridgeParticlesPlaying = false;
            _bulletCartridgeParticles.Stop();
        }
    }
}
