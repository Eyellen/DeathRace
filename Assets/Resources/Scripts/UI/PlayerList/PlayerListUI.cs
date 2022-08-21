using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] private GameObject _playerListContainer;
    [SerializeField] private GameObject _playerInfoBarTemplatePrefab;

    private Dictionary<Player, GameObject> _playerInfoBars = new Dictionary<Player, GameObject>();

    private void Start()
    {
        InitializePlayerList();
        PlayerListManager.Instance.OnPlayerAddedToList += AddPlayerToList;
        PlayerListManager.Instance.OnPlayerRemovedFromList += RemovePlayerFromList;
    }

    private void AddPlayerToList(Player player)
    {
        GameObject playerInfoBar = Instantiate(_playerInfoBarTemplatePrefab, _playerListContainer.transform);
        _playerInfoBars[player] = playerInfoBar;
    }

    private void RemovePlayerFromList(Player player)
    {
        Destroy(_playerInfoBars[player]);
        _playerInfoBars.Remove(player);
    }

    private void InitializePlayerList()
    {
        Player[] allPlayers = PlayerListManager.Instance.AllPlayers.ToArray();

        foreach (var player in allPlayers)
        {
            AddPlayerToList(player);
        }
    }
}
