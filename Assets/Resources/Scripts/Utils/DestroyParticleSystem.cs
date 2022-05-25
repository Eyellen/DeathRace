using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleSystem : MonoBehaviour
{
    private ParticleSystem[] _particles;

    private void Awake()
    {
        _particles = transform.GetComponentsInChildren<ParticleSystem>();
    }


    private void Update()
    {
        CheckForDestroy();
    }

    private void CheckForDestroy()
    {
        foreach (ParticleSystem particle in _particles)
        {
            if (particle != null) return;
        }
        Destroy(gameObject);
    }
}
