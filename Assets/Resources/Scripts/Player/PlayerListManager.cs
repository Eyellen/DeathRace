using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerListManager : NetworkBehaviour
{
    public static PlayerListManager Instance { get; private set; }

    public List<Player> AllPlayers { get; private set; } = new List<Player>();

    public Action<Player> OnPlayerAddedToList;
    public Action<Player> OnPlayerRemovedFromList;

    private void Awake()
    {
        InitializeInstance();
        Player.OnPlayerJoin += AddPlayerToList;
        Player.OnPlayerExit += RemovePlayerFromList;
    }

    private void InitializeInstance()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void AddPlayerToList(Player player)
    {
        AllPlayers.Add(player);
        OnPlayerAddedToList?.Invoke(player);
    }

    private void RemovePlayerFromList(Player player)
    {
        AllPlayers.Remove(player);
        OnPlayerRemovedFromList?.Invoke(player);
    }
}
