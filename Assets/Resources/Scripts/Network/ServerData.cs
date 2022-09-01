using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public static class ServerData
{
    public static string ServerName { get; set; }
    public static int GameModeIndex { get; set; }
    public static GameModeDataBase CurrentGameModeData { get; set; }
    //public static string Password { get; set; }
    public static int MaxPlayersCount
    {
        get { return NetworkManager.singleton.maxConnections; }
        set { NetworkManager.singleton.maxConnections = value; }
    }
    public static int CurrentPlayersCount
    {
        get => NetworkManager.singleton.numPlayers;
    }
    public static int MaxPing { get; set; }
    //public static string Region { get; set; }
}
