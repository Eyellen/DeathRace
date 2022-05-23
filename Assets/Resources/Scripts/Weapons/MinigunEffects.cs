using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particles;
    [SerializeField] private Minigun _minigun;
    private bool _isEmitting;

    void Start()
    {
        
    }


    void Update()
    {
        ShootEffect();
    }

    private void ShootEffect()
    {
        if(_minigun.IsShooting && !_isEmitting)
        {
            _isEmitting = true;
            foreach (var particle in _particles)
            {
                var emission = particle.emission;
                emission.enabled = true;
            }
        }

        if(!_minigun.IsShooting && _isEmitting)
        {
            _isEmitting = false;
            foreach (var particle in _particles)
            {
                var emission = particle.emission;
                emission.enabled = false;
            }
        }
    }
}
