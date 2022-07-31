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

    // Index of selected car, value -1 means that player is spectating
    [field: SyncVar]
    public int SelectedCarIndex { get; private set; } = 0;

    // Reference to the car if it's been spawned
    [field: SyncVar]
    public GameObject Car { get; set; }

    [field: SerializeField]
    [field: SyncVar]
    public string Username { get; private set; }

    public override void OnStartClient()
    {
        CameraTransform = transform.Find("Camera");
        CameraManager = GetComponent<CameraManager>();

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
    }

    [Command]
    private void CmdSetUsername(string username)
    {
        Username = username;
    }

    [Command]
    public void CmdSetSelectedCarIndex(int carIndex)
    {
        SelectedCarIndex = carIndex;
    }
}
