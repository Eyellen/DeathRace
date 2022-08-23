using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoBarUI : MonoBehaviour
{
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
    public string PlayerStatus { get => Enum.Parse(typeof(), _playerStatus.text); }
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
        LapsCompleted = _player.SessionStats.LapsCompleted;
        Kills = _player.SessionStats.Kills;
    }
}
