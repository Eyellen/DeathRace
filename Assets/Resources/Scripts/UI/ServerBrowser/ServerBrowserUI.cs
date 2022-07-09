using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;

public class ServerBrowserUI : MonoBehaviour
{
    [SerializeField] private NewNetworkDiscovery _networkDiscovery;
    [SerializeField] private GameObject _serversListArea;
    [SerializeField] private GameObject _serverInfoBarTemplatePrefab;

    private void OnValidate()
    {
        if(_networkDiscovery == null)
        {
            _networkDiscovery = FindObjectOfType<NewNetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(_networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, _networkDiscovery }, "Set NetworkDiscovery");
        }
    }

    public void OnDiscoveredServer(DiscoveryResponse info)
    {
        Instantiate(_serverInfoBarTemplatePrefab, _serversListArea.transform);
        Debug.Log(info.ServerName);
        Debug.Log(info.MaxPlayersCount);
        Debug.Log(info.CurrentPlayersCount);
        Debug.Log(info.MaxPing);
    }

    private IEnumerator ClearServerList()
    {
        while (_serversListArea.transform.childCount > 0)
        {
            Destroy(_serversListArea.transform.GetChild(0).gameObject);
            yield return null;
        }
    }
}
