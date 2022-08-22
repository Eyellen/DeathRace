using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] private GameObject _playerListContainer;
    [SerializeField] private GameObject _playerInfoBarTemplatePrefab;

    [SerializeField] private TextMeshProUGUI _gameModeText;
    [SerializeField] private TextMeshProUGUI _gameModeInfoText;

    private Dictionary<Player, PlayerInfoBarUI> _playerInfoBars = new Dictionary<Player, PlayerInfoBarUI>();

    private void Start()
    {
        InitializePlayerList();
        PlayerListManager.Instance.OnPlayerAddedToList += AddPlayerToList;
        PlayerListManager.Instance.OnPlayerRemovedFromList += RemovePlayerFromList;
        AsignGameModeInfo();
    }

    private void AsignGameModeInfo()
    {
        switch (GameManager.Instance.CurrentGameMode)
        {
            case GameMode.Free:
                {
                    _gameModeText.text = "Free Mode";
                    _gameModeInfoText.text = string.Empty;
                    break;
                }
            case GameMode.Race:
                {
                    _gameModeText.text = "Race Mode";
                    RaceModeManager raceModeManager = GameModeBase.Instance as RaceModeManager;
                    _gameModeInfoText.text = $"Laps to win: {raceModeManager.LapsToWin}\t" +
                        $"Tiles activate on lap: {raceModeManager.ActivateTilesOnLap}\n" +
                        $"Tiles cooldown: {raceModeManager.TilesCooldown} seconds";
                    break;
                }
            default:
                {
                    break;
                }
        }
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
