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

    private IEnumerator _cooldownCoroutine;

    [SerializeField] private Light _tileLight;

    protected virtual void Start()
    {
        if (IsReady) _tileLight.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag != "Car") return;

        if (!IsReady) return;

        // To prevent caling on server, only by Command
        if (!other.transform.root.GetComponent<CarBase>().hasAuthority) return;

        CmdOnTriggerEnter();

        OnCarEnter(other.transform.root.gameObject);
    }

    [Command(requiresAuthority = false)]
    private void CmdOnTriggerEnter()
    {
        if (!IsReady) return;
        SetReady(false);

        _cooldownCoroutine = CooldownCoroutine();
        StartCoroutine(_cooldownCoroutine);
        //if (_cooldownCoroutine != null)
        //    StopCoroutine(_cooldownCoroutine);
        //_cooldownCoroutine = CooldownCoroutine();
        //StartCoroutine(_cooldownCoroutine);
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
        OnTileCooledDown();
    }

    [Server]
    public void SetReady(bool isReady)
    {
        IsReady = isReady;
        RpcToggleLight(isReady);

        if (!isReady)
        {
            if (_cooldownCoroutine != null)
                StopCoroutine(_cooldownCoroutine);

            OnTileDeactivated();
        }
    }

    protected abstract void OnCarEnter(GameObject car);

    protected virtual void OnTileCooledDown() { }

    protected virtual void OnTileDeactivated() { }
}
