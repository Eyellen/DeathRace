using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SettingsLoadManager : MonoBehaviour
{
    private void Start()
    {
        SettingsSaveSystem.Load();
    }
}
