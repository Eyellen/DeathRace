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
    [SerializeField] private TextMeshProUGUI _lapsText;
    [SerializeField] private TextMeshProUGUI _killsText;

    public string Username { get => _usernameText.text; private set => _usernameText.text = value; }
    public int LapsCompleted { get => int.Parse(_usernameText.text); private set => _lapsText.text = value.ToString(); }
    public int Kills { get => int.Parse(_killsText.text); private set => _killsText.text = value.ToString(); }

    private void Start()
    {
        Player.OnNameSynced += UpdateUsername;
    }

    private void UpdateUsername(string newName)
    {
        Username = newName;
    }
}
