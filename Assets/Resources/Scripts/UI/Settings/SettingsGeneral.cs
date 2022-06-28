using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class designed to refer to all settings classes that need to be saved
/// </summary>
[DisallowMultipleComponent]
public class SettingsGeneral : MonoBehaviour
{
    [field: SerializeField] public SettingsGraphics SettingsGraphics { get; private set; }
    [field: SerializeField] public SettingsAudio SettingsAudio { get; private set; }
    [field: SerializeField] public SettingsUser SettingsUser { get; private set; }

    public void SaveSettings()
    {
        SettingsSaveSystem.Save(this);
    }
}
