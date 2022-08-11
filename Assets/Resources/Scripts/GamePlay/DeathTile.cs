using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DeathTile : TileBase
{
    [SerializeField] private GameObject _spikes;
    private Transform _spikesTransform;

    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minHeight;

    private IEnumerator _spikesCoroutine;

    private void Start()
    {
        _spikesTransform = _spikes.transform;
    }

    protected override void OnCarEnter(GameObject car)
    {
        CmdRaiseSpikes();
    }

    [Server]
    protected override void OnTileReset()
    {
        SetReady(false);
        CmdLowerSpikes();
    }

    [Command(requiresAuthority = false)]
    private void CmdRaiseSpikes()
    {
        if (_spikesCoroutine != null)
            StopCoroutine(_spikesCoroutine);

        _spikesCoroutine = RaiseSpikesCoroutine();
        StartCoroutine(_spikesCoroutine);
    }

    [Command(requiresAuthority = false)]
    private void CmdLowerSpikes()
    {
        if (_spikesCoroutine != null)
            StopCoroutine(_spikesCoroutine);

        _spikesCoroutine = LowerSpikesCoroutine();
        StartCoroutine(LowerSpikesCoroutine());
    }

    [Server]
    private IEnumerator RaiseSpikesCoroutine()
    {
        while (_spikesTransform.position.y < _maxHeight)
        {
            _spikesTransform.position += Vector3.up * Time.deltaTime;

            yield return null;
        }
    }

    [Server]
    private IEnumerator LowerSpikesCoroutine()
    {
        while (_spikesTransform.position.y > _minHeight)
        {
            _spikesTransform.position -= Vector3.up * Time.deltaTime;

            if (_spikesTransform.position.y < _minHeight)
                SetReady(true);

            yield return null;
        }
    }
}
