using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;

public class ServerDiscoveryUI : MonoBehaviour
{
    [SerializeField] private NewNetworkDiscovery _networkDiscovery;
    [SerializeField] private GameObject _serversListArea;
    [SerializeField] private GameObject _serverInfoBarTemplatePrefab;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(_networkDiscovery == null)
        {
            _networkDiscovery = FindObjectOfType<NewNetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(_networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new Object[] { this, _networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif

    public void FindServers()
    {
        ClearServerList();
        _networkDiscovery.StartDiscovery();
    }

    public void OnDiscoveredServer(DiscoveryResponse info)
    {
        Debug.Log(info.ServerName);
        Debug.Log(info.MaxPlayersCount);
        Debug.Log(info.CurrentPlayersCount);
        Debug.Log(info.MaxPing);

        var serverInfoBar = Instantiate(_serverInfoBarTemplatePrefab, _serversListArea.transform).GetComponent<ServerInfoBarUI>();

        serverInfoBar.ServerName = info.ServerName;
        serverInfoBar.PlayersCount = $"{info.CurrentPlayersCount}/{info.MaxPlayersCount}";
        serverInfoBar.MaxPing = info.MaxPing.ToString();
        //serverInfoBar.Region = info.Region;
    }

    private IEnumerator ClearServerListCoroutine()
    {
        while (_serversListArea.transform.childCount > 0)
        {
            Destroy(_serversListArea.transform.GetChild(0).gameObject);
            yield return null;
        }
    }

    private void ClearServerList()
    {
        for (int i = _serversListArea.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_serversListArea.transform.GetChild(i).gameObject);
        }
    }
}
