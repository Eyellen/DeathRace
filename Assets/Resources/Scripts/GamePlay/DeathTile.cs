using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTile : TileBase
{
    [SerializeField] private GameObject _spikes;

    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minHeight;

    private IEnumerator _spikesCoroutine;

    protected override void OnCarEnter(GameObject car)
    {
        if (_spikesCoroutine != null)
            StopCoroutine(_spikesCoroutine);

        _spikesCoroutine = RaiseSpikes();
        StartCoroutine(_spikesCoroutine);
    }

    protected override void OnTileReset()
    {
        if (_spikesCoroutine != null)
            StopCoroutine(_spikesCoroutine);

        _spikesCoroutine = LowerSpikes();
        StartCoroutine(LowerSpikes());
    }

    private IEnumerator RaiseSpikes()
    {
        var spikesTransform = _spikes.transform;

        while (spikesTransform.position.y < _maxHeight)
        {
            spikesTransform.position += Vector3.up * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator LowerSpikes()
    {
        var spikesTransform = _spikes.transform;

        while (spikesTransform.position.y > _minHeight)
        {
            spikesTransform.position -= Vector3.up * Time.deltaTime;

            yield return null;
        }
    }
}
