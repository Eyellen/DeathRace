using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[DisallowMultipleComponent]
public class Player : NetworkBehaviour
{
    public static Player LocalPlayer { get; private set; }

    public Transform CameraTransform { get; private set; }

    public CameraManager CameraManager { get; private set; }

    public PlayerSessionStats SessionStats { get; private set; }

    public static Player[] AllPlayers
    {
        //get => FindObjectsOfType<Player>();
        get => PlayerListManager.Instance.AllPlayers.ToArray();
    }

    /// <summary>
    /// Players that not spectating
    /// </summary>
    public static List<Player> ActivePlayers
    {
        get
        {
            Player[] allPlayers = AllPlayers;

            List<Player> activePlayers = new List<Player>();
            foreach (var player in allPlayers)
            {
                if (player.SelectedCarIndex == -1) continue;

                activePlayers.Add(player);
            }
            return activePlayers;
        }
    }

    // Index of selected car, value -1 means that player is spectating
    [field: SyncVar]
    public int SelectedCarIndex { get; private set; } = 0;

    // Reference to the car if it's been spawned
    [field: SyncVar]
    public GameObject Car { get; set; }

    [field: SerializeField]
    [field: SyncVar(hook = nameof(OnNameSyncedHook))]
    public string Username { get; private set; }

    public static Action<Player> OnPlayerJoin;
    public static Action<Player> OnPlayerExit;

    public Action<string> OnNameSynced;

    public Action<string> OnNameUpdated;

    public override void OnStartClient()
    {
        CameraTransform = transform.Find("Camera");
        CameraManager = GetComponent<CameraManager>();
        SessionStats = GetComponent<PlayerSessionStats>();

        if (isLocalPlayer)
        {
            LocalPlayer = this;
            CmdSetUsername(Username = SettingsUser.Username);
            CameraTransform.gameObject.tag = "MainCamera";
        }
        else
        {
            CameraTransform.gameObject.SetActive(false);
        }

        StartCoroutine(OnPlayerJoinedCoroutine());
    }

    private void OnDestroy()
    {
        OnPlayerExit?.Invoke(this);

        // Setting null to static events to prevent error
        // But setting only if general count of players is null
        // Otherwise it will cause issues on existing Player instances
        if(PlayerListManager.Instance.AllPlayers.Count <= 0)
        {
            OnPlayerJoin = null;
            OnPlayerExit = null;
        }
    }

    [Command]
    private void CmdSetUsername(string username)
    {
        List<string> otherPlayersNames = new List<string>();
        //Debug.Log("All usernames");
        foreach (var player in AllPlayers)
        {
            otherPlayersNames.Add(player.Username);
            //Debug.Log(player.Username);
        }

        username = ServerUsernameRequirements.CheckIfNameIsUnique(username, otherPlayersNames.ToArray());
        //Debug.Log(username);

        Username = username;
        OnNameUpdated?.Invoke(username);
    }

    [Command]
    public void CmdSetSelectedCarIndex(int carIndex)
    {
        SelectedCarIndex = carIndex;
    }

    public void OnNameSyncedHook(string oldName, string newName)
    {
        OnNameSynced?.Invoke(newName);
    }

    private IEnumerator OnPlayerJoinedCoroutine()
    {
        while (Username == null || Username == string.Empty)
            yield return null;

        OnPlayerJoin?.Invoke(this);
    }
}
