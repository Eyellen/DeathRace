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

    [field: SerializeField]
    [field: SyncVar]
    public string Username { get; private set; }

    private void Start()
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
}
