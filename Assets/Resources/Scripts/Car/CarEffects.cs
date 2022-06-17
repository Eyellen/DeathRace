using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{
    [SerializeField] private Minigun _minigun;

    [Header("Bullet cartridge particles")]
    [SerializeField] private ParticleSystem _bulletCartridgeParticles;
    private bool _isBulletCartridgeParticlesPlaying;

    void Update()
    {
        BulletCartridgeEffect();
    }

    private void BulletCartridgeEffect()
    {
        if(_minigun.IsShooting && !_isBulletCartridgeParticlesPlaying)
        {
            _isBulletCartridgeParticlesPlaying = true;
            _bulletCartridgeParticles.Play();
        }
        if (!_minigun.IsShooting && _isBulletCartridgeParticlesPlaying)
        {
            _isBulletCartridgeParticlesPlaying = false;
            _bulletCartridgeParticles.Stop();
        }
    }
}
