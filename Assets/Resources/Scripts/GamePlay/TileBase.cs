using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class TileBase : NetworkBehaviour
{
    [field: SyncVar]
    public bool IsReady { get; private set; } = true;

    private int _cooldown = 5;

    [SerializeField] private Light _tileLight;

    [Server]
    public void InitializeTile(int cooldown)
    {
        _cooldown = cooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag != "Car") return;

        if (!IsReady) return;

        CmdOnTriggerEnter();

        OnCarEnter(other.transform.root.gameObject);
    }

    [Command(requiresAuthority = false)]
    private void CmdOnTriggerEnter()
    {
        if (!IsReady) return;
        SetReady(false);
        StartCoroutine(Cooldown());
    }

    [ClientRpc]
    private void RpcToggleLight(bool isEnabled)
    {
        _tileLight.enabled = isEnabled;
    }

    [Server]
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldown);

        SetReady(true);
        OnTileReset();
    }

    [Server]
    protected void SetReady(bool isReady)
    {
        IsReady = isReady;
        RpcToggleLight(isReady);
    }

    protected abstract void OnCarEnter(GameObject car);

    protected virtual void OnTileReset() { }
}
