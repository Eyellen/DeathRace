using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{
    [SerializeField] private Minigun _minigun;

    [Header("Bullet cartridge particles")]
    [SerializeField] private GameObject _bulletCartridgeParticlesPrefab;
    [SerializeField] private Transform _cartridgeSpawnPoint;
    [SerializeField] private float _delayBetweenCartridges;
    private float _timeSinceLastCartridgeSpawned;

    void Update()
    {
        BulletCartridgeEffect();
    }

    private void BulletCartridgeEffect()
    {
        if (Time.time <= _timeSinceLastCartridgeSpawned + _delayBetweenCartridges) return;

        if (!_minigun.IsShooting) return;

        _timeSinceLastCartridgeSpawned = Time.time;
        Instantiate(_bulletCartridgeParticlesPrefab, _cartridgeSpawnPoint);
    }
}
