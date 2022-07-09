using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerManagerUI : MonoBehaviour
{
    private NetworkManager _networkManager;
    private NewNetworkDiscovery _networkDiscovery;

    private void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
        _networkDiscovery = FindObjectOfType<NewNetworkDiscovery>();
    }

    public void Host()
    {
        _networkManager.StartHost();
        _networkDiscovery.AdvertiseServer();
    }

    public void StopHostOrClient()
    {
        if (NetworkClient.localPlayer.isServer)
        {
            _networkManager.StopHost();
        }
        else
        {
            _networkManager.StopClient();
        }

        _networkDiscovery.StopDiscovery();
    }
}
