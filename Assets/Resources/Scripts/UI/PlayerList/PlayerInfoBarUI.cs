using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoBarUI : MonoBehaviour
{
    [SerializeField] private Color _spectatingColor = new Color32(212, 212, 212, 255);
    [SerializeField] private Color _aliveColor = new Color32(21, 255, 0, 255);
    [SerializeField] private Color _eliminatedColor = new Color32(255, 0, 0, 255);

    private Player _player;
    public Player Player
    {
        get => _player;
        set
        {
            _player = value;

            Username = _player.Username;
        }
    }

    [SerializeField] private TextMeshProUGUI _usernameText;
    [SerializeField] private TextMeshProUGUI _playerStatus;
    [SerializeField] private TextMeshProUGUI _lapsText;
    [SerializeField] private TextMeshProUGUI _killsText;

    public string Username { get => _usernameText.text; private set => _usernameText.text = value; }
    public PlayerStatus PlayerStatus 
    { 
        get => (PlayerStatus)Enum.Parse(typeof(PlayerStatus), _playerStatus.text);
        set
        {
            _playerStatus.text = value.ToString();
            switch (value)
            {
                case PlayerStatus.Spectator:
                    _playerStatus.color = _spectatingColor;
                    break;
                case PlayerStatus.Alive:
                    _playerStatus.color = _aliveColor;
                    break;
                case PlayerStatus.Eliminated:
                    _playerStatus.color = _eliminatedColor;
                    break;
            }
        }
    }
    public int LapsCompleted { get => int.Parse(_usernameText.text); private set => _lapsText.text = value.ToString(); }
    public int Kills { get => int.Parse(_killsText.text); private set => _killsText.text = value.ToString(); }

    private IEnumerator _updateCoroutine;

    private void Start()
    {
        UpdateAllFields();
        Player.OnNameSynced += UpdateUsername;
    }

    private void UpdateUsername(string newName)
    {
        Username = newName;
    }

    private void OnEnable()
    {
        UpdateAllFields();

        if (_updateCoroutine != null)
            StopCoroutine(_updateCoroutine);
        _updateCoroutine = UpdateCoroutine();
        StartCoroutine(_updateCoroutine);
    }

    private void OnDisable()
    {
        if (_updateCoroutine != null)
            StopCoroutine(_updateCoroutine);
    }

    private IEnumerator UpdateCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.2f);

            UpdateAllFields();

            yield return null;
        }
    }

    private void UpdateAllFields()
    {
        Username = _player.Username;
        PlayerStatus = _player.SessionStats.PlayerStatus;
        LapsCompleted = _player.SessionStats.LapsCompleted;
        Kills = _player.SessionStats.Kills;
    }
}
