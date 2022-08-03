using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// TODO: Add an Initializer method with parameter type of DiscoveryResponse
public class ServerInfoBarUI : MonoBehaviour
{
    public ServerDiscoveryUI ServerDiscoveryUI { get; set; }

    [SerializeField] private TextMeshProUGUI _gameModeText;
    [SerializeField] private TextMeshProUGUI _serverNameText;
    [SerializeField] private TextMeshProUGUI _playersCountText;
    [SerializeField] private TextMeshProUGUI _maxPingText;
    [SerializeField] private TextMeshProUGUI _RegionText;

    private int _gameModeIndex;

    public long ServerId { get; set; }
    public Uri Uri { get; set; }

    public int GameModeIndex
    {
        get => _gameModeIndex;
        set
        {
            _gameModeIndex = value;
            _gameModeText.text = ((GameMode)_gameModeIndex).ToString();
        }
    }
    public string ServerName { get { return _serverNameText.text; } set { _serverNameText.text = value; } }
    public string PlayersCount { get { return _playersCountText.text; } set { _playersCountText.text = value; } }
    public string MaxPing { get { return _maxPingText.text; } set { _maxPingText.text = value; } }
    public string Region { get { return _RegionText.text; } set { _RegionText.text = value; } }

    private float _lastResponseTime;
    /// <summary>
    /// Time to reconnect in seconds.
    /// If server not responding anymore it has this time to start responding again.
    /// Otherwise this ServerBar will be destroyed.
    /// </summary>
    private float _timeToReconnect = 7;

    private DiscoveryResponse _discoveryResponse;

    public DiscoveryResponse DiscoveryResponse
    {
        get => _discoveryResponse;
        set
        {
            _discoveryResponse = value;

            _lastResponseTime = Time.time;

            ServerId = _discoveryResponse.serverId;
            Uri = _discoveryResponse.uri;

            ServerName = _discoveryResponse.ServerName;
            GameModeIndex = _discoveryResponse.GameModeIndex;
            PlayersCount = $"{_discoveryResponse.CurrentPlayersCount}/{_discoveryResponse.MaxPlayersCount}";
            MaxPing = _discoveryResponse.MaxPing.ToString();
            //Region = _discoveryResponse.Region;
        }
    }

    private void Update()
    {
        if (Time.time > _lastResponseTime + _timeToReconnect)
            //gameObject.SetActive(false);
            Destroy(gameObject);
    }

    public void SetAddress()
    {
        ServerDiscoveryUI.SelectedServerUri = Uri;
        SetId();
    }

    private void SetId()
    {
        ServerDiscoveryUI.SelectedServerId = ServerId;
    }    
}
