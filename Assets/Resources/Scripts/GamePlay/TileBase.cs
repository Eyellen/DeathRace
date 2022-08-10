using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : MonoBehaviour
{
    private bool _isReady = true;
    private int _cooldown = 5;

    [SerializeField] private Light _tileLight;

    public void InitializeTile(int cooldown)
    {
        _cooldown = cooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag != "Car") return;

        if (!_isReady) return;
        _isReady = false;
        _tileLight.enabled = false;
        StartCoroutine(Cooldown());

        OnCarEnter(other.transform.root.gameObject);
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldown);

        _isReady = true;
        _tileLight.enabled = true;
        OnTileReset();
    }

    protected abstract void OnCarEnter(GameObject car);

    protected virtual void OnTileReset() { }
}
