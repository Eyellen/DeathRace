using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class designed to refer to all settings classes that need to be saved
/// </summary>
[DisallowMultipleComponent]
public class SettingsGeneralUI : MonoBehaviour
{
    public void SaveSettings()
    {
        SettingsSaveSystem.Save();
    }
}
