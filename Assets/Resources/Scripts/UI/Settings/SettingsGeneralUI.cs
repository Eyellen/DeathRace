using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class designed to refer to all settings classes that need to be saved
/// </summary>
[DisallowMultipleComponent]
public class SettingsGeneralUI : MonoBehaviour
{
    [field: SerializeField] public SettingsGraphicsUI SettingsGraphics { get; private set; }
    [field: SerializeField] public SettingsAudioUI SettingsAudio { get; private set; }
    [field: SerializeField] public SettingsUserUI SettingsUser { get; private set; }

    public void SaveSettings()
    {
        SettingsSaveSystem.Save(this);
    }
}
