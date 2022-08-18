using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] private GameObject _playerListContent;

    [SerializeField] private GameObject _playerInfoBarTemplatePrefab;

    private void Start()
    {
        //InitializePlayerList();
    }

    private void OnEnable()
    {
        InitializePlayerList();
    }

    private void OnDisable()
    {
        ClearPlayerList();
    }

    private void InitializePlayerList()
    {
        Player[] players = FindObjectsOfType<Player>();

        foreach (var player in players)
        {
            var playerInfoBar = Instantiate(_playerInfoBarTemplatePrefab, _playerListContent.transform);
            playerInfoBar.transform.Find("Username").GetComponent<TextMeshProUGUI>().text = player.Username;
        }
    }

    private void ClearPlayerList()
    {
        for (int i = _playerListContent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(_playerListContent.transform.GetChild(i).gameObject);
        }
    }
}
