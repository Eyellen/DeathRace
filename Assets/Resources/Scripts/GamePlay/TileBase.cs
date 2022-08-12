using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class TileBase : NetworkBehaviour
{
    [field: SyncVar]
    public bool IsReady { get; private set; } = true;

    public int Cooldown { get; set; } = 5;

    [SerializeField] private Light _tileLight;

    protected virtual void Start()
    {
        if (IsReady) _tileLight.enabled = true;
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
        StartCoroutine(CooldownCoroutine());
    }

    [ClientRpc]
    private void RpcToggleLight(bool isEnabled)
    {
        _tileLight.enabled = isEnabled;
    }

    [Server]
    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(Cooldown);

        SetReady(true);
        OnTileReset();
    }

    [Server]
    public void SetReady(bool isReady)
    {
        IsReady = isReady;
        RpcToggleLight(isReady);
    }

    protected abstract void OnCarEnter(GameObject car);

    protected virtual void OnTileReset() { }
}
