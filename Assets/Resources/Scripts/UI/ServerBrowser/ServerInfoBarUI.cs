using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerInfoBarUI : MonoBehaviour
{
    public ServerDiscoveryUI ServerDiscoveryUI { get; set; }

    [SerializeField] private TextMeshProUGUI _serverNameText;
    [SerializeField] private TextMeshProUGUI _playersCountText;
    [SerializeField] private TextMeshProUGUI _maxPingText;
    [SerializeField] private TextMeshProUGUI _RegionText;

    public long ServerId { get; set; }
    public Uri Uri { get; set; }

    public string ServerName { get { return _serverNameText.text; } set { _serverNameText.text = value; } }
    public string PlayersCount { get { return _playersCountText.text; } set { _playersCountText.text = value; } }
    public string MaxPing { get { return _maxPingText.text; } set { _maxPingText.text = value; } }
    public string Region { get { return _RegionText.text; } set { _RegionText.text = value; } }

    public void SetAddress()
    {
        ServerDiscoveryUI.SelectedServerUri = Uri;
    }
}
