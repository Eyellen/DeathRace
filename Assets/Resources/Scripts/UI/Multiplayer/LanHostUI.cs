using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LanHostUI : MonoBehaviour
{
    private NetworkManager _networkManager;

    private void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
    }

    public void Host()
    {
        _networkManager.StartHost();
    }

    public void Join()
    {
        _networkManager.StartClient();
    }

    public void SetIP(string ip)
    {
        _networkManager.networkAddress = ip;
    }

    public void StopHostOrClient()
    {
        if(NetworkClient.localPlayer.isServer)
        {
            _networkManager.StopHost();
        }
        else
        {
            _networkManager.StopClient();
        }
    }
}
