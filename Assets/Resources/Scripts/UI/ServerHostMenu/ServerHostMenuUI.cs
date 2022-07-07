using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServerHostMenuUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _serverNameField;
    [SerializeField] private TMP_InputField _passwordField;
    [SerializeField] private Slider _maxPlayersSlider;
    [SerializeField] private TextMeshProUGUI _maxPlayersText;
    [SerializeField] private Slider _maxPingSlider;
    [SerializeField] private TextMeshProUGUI _maxPingText;

    private void Start()
    {
        RefreshOptions();
    }

    public void SetServerName(string name)
    {
        ServerData.ServerName = name;
    }

    public void SetPassword(string pass)
    {
        //
    }

    public void SetMaxPlayersCount(float maxCount)
    {
        ServerData.MaxPlayersCount = (int)maxCount;
        _maxPlayersText.text = maxCount.ToString();
    }

    public void SetMaxPing(float maxPing)
    {
        if((int)maxPing >= 11)
        {
            ServerData.MaxPing = 1000;
            _maxPingText.text = "No Limit";
            return;
        }

        ServerData.MaxPing = 50 + (int)maxPing * 25;
        _maxPingText.text = ServerData.MaxPing.ToString();
    }

    public void RefreshOptions()
    {
        _serverNameField.text = ServerData.ServerName;
        //_passwordField.text = ServerData.;
        _maxPlayersSlider.value = ServerData.MaxPlayersCount;
        _maxPingSlider.value = ServerData.MaxPing;
    }
}
