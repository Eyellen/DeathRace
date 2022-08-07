using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Material _notPassedMaterial;
    [SerializeField] private Material _passedMaterial;

    [SerializeField] private ParticleSystemRenderer _rightBoundParticlesRenderer;
    [SerializeField] private ParticleSystemRenderer _leftBoundParticlesRenderer;

    [field: SerializeField] public Collider Trigger { get; private set; }

    public int CheckPointIndex { get; set; }
    public bool IsPassed { get; private set; }

    public Action<int> OnCheckPointPassed;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag != "Car") return;
        if (!other.transform.root.GetComponent<CarBase>().netIdentity.hasAuthority) return;

        //Debug.Log($"Car entered the CheckPoint trigger by index {CheckPointIndex}");

        OnCheckPointPassed?.Invoke(CheckPointIndex);
    }
}
