using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;

public class ServerDiscoveryUI : MonoBehaviour
{
    public static ServerDiscoveryUI Instance { get; private set; }

    [SerializeField] private NewNetworkDiscovery _networkDiscovery;
    [SerializeField] private GameObject _serversListArea;
    [SerializeField] private GameObject _serverInfoBarTemplatePrefab;

    private Dictionary<long, ServerInfoBarUI> _discoveredServers = new Dictionary<long, ServerInfoBarUI>();
    public long SelectedServerId { get; set; }
    public Uri SelectedServerUri { get; set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if(_networkDiscovery == null)
        {
            _networkDiscovery = FindObjectOfType<NewNetworkDiscovery>();
            UnityEditor.Events.UnityEventTools.AddPersistentListener(_networkDiscovery.OnServerFound, OnDiscoveredServer);
            UnityEditor.Undo.RecordObjects(new UnityEngine.Object[] { this, _networkDiscovery }, "Set NetworkDiscovery");
        }
    }
#endif

    private void OnEnable()
    {
        _networkDiscovery.StartDiscovery();
    }

    private void OnDisable()
    {
        _networkDiscovery.StopDiscovery();
    }

    public void FindServers()
    {
        ClearServerList();
        _networkDiscovery.StartDiscovery();
    }

    public void OnDiscoveredServer(DiscoveryResponse info)
    {
        ServerInfoBarUI serverInfoBar;
        if (_discoveredServers.ContainsKey(info.serverId))
        {
            serverInfoBar = _discoveredServers[info.serverId];
        }
        else
        {
            serverInfoBar = Instantiate(_serverInfoBarTemplatePrefab, _serversListArea.transform).GetComponent<ServerInfoBarUI>();
        }
        serverInfoBar.ServerDiscoveryUI = this;
        serverInfoBar.DiscoveryResponse = info;
        _discoveredServers[info.serverId] = serverInfoBar;
    }

    private void ClearServerList()
    {
        _discoveredServers.Clear();
        for (int i = _serversListArea.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_serversListArea.transform.GetChild(i).gameObject);
        }
    }

    public void Connect()
    {
        if (SelectedServerUri == null) return;

        LoadingScreenManager.Instance.Enable();

        _networkDiscovery.StopDiscovery();
        InitializeServerDataForConnection();
        NetworkManager.singleton.StartClient(SelectedServerUri);
    }

    private void InitializeServerDataForConnection()
    {
        ServerData.GameModeIndex = _discoveredServers[SelectedServerId].GameModeIndex;
        ServerData.ServerName = _discoveredServers[SelectedServerId].ServerName;
        ServerData.MaxPing = int.Parse(_discoveredServers[SelectedServerId].MaxPing);
    }
}
