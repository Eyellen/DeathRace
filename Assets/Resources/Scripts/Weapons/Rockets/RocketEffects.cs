using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rocket))]
public class RocketEffects : MonoBehaviour
{
    private Rocket _rocket;

    [Header("Smoke effect")]
    [SerializeField] private Transform _smokeEffectTransform;

    [Header("Explosion effect")]
    [SerializeField] private GameObject _explosionParticlesPrefab;

    private void Awake()
    {
        _rocket = GetComponent<Rocket>();
    }

    void Start()
    {
        _rocket.OnRocketExplode += SeparateSmokeFromRocket;
    }

    private void SeparateSmokeFromRocket()
    {
        _smokeEffectTransform.parent = null;

        ParticleSystem[] particles = _smokeEffectTransform.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particle in particles)
        {
            var main = particle.main;
            main.loop = false;
        }

        SpawnExplosionEffect();
    }

    private void SpawnExplosionEffect()
    {
        Instantiate(_explosionParticlesPrefab, transform.position, Quaternion.identity);
    }
}
