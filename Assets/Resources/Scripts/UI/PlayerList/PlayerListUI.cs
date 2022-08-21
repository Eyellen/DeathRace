using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] private GameObject _playerListContainer;
    [SerializeField] private GameObject _playerInfoBarTemplatePrefab;

    private Dictionary<Player, PlayerInfoBarUI> _playerInfoBars = new Dictionary<Player, PlayerInfoBarUI>();

    private void Start()
    {
        InitializePlayerList();
        PlayerListManager.Instance.OnPlayerAddedToList += AddPlayerToList;
        PlayerListManager.Instance.OnPlayerRemovedFromList += RemovePlayerFromList;
    }

    private void AddPlayerToList(Player player)
    {
        GameObject playerInfoBar = Instantiate(_playerInfoBarTemplatePrefab, _playerListContainer.transform);

        PlayerInfoBarUI playerInfoBarComponent = playerInfoBar.GetComponent<PlayerInfoBarUI>();
        _playerInfoBars[player] = playerInfoBarComponent;

        playerInfoBarComponent.Player = player;
    }

    private void RemovePlayerFromList(Player player)
    {
        Destroy(_playerInfoBars[player].gameObject);
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
