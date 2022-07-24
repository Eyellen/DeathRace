using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Material _notPassedMaterial;
    [SerializeField] private Material _passedMaterial;

    [SerializeField] private ParticleSystemRenderer _rightBoundParticlesRenderer;
    [SerializeField] private ParticleSystemRenderer _leftBoundParticlesRenderer;

    [field: SerializeField] public Collider Trigger { get; private set; }

    public bool IsPassed { get; private set; }

    public void MarkAsPassed()
    {
        IsPassed = true;
        _rightBoundParticlesRenderer.material = _passedMaterial;
        _leftBoundParticlesRenderer.material = _passedMaterial;
    }

    public void ResetPoint()
    {
        IsPassed = false;
        _rightBoundParticlesRenderer.material = _notPassedMaterial;
        _leftBoundParticlesRenderer.material = _notPassedMaterial;
    }
}
