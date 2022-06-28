using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SettingsSaveSystem
{
    private const string FileName = "/Settings.config";

    #region CachedSave
    private static SettingsGeneralData _cachedSave;
    public static SettingsGeneralData CachedSave
    {
        get
        {
            if (_cachedSave == null)
            {
                Load();
            }
            return _cachedSave;
        }
        private set
        {
            _cachedSave = value;
        }
    }

    private static void UpdateCachedSave(SettingsGeneral settingsGeneral)
    {
        CachedSave = new SettingsGeneralData(settingsGeneral);
    }
    #endregion

    public static void Save(SettingsGeneral settingsGeneral)
    {
        string path = Application.persistentDataPath + FileName;

        using (var stream = new FileStream(path, FileMode.Create))
        {
            var formatter = new BinaryFormatter();

            var data = new SettingsGeneralData(settingsGeneral);

            formatter.Serialize(stream, data);
        }
#if UNITY_EDITOR
        Debug.Log("SettingsData has beem saved successfully");
#endif
        UpdateCachedSave(settingsGeneral);
    }

    public static SettingsGeneralData Load()
    {
        string path = Application.persistentDataPath + FileName;

        if(!File.Exists(path))
        {
#if UNITY_EDITOR
            Debug.LogError("Trying to access SettingsSave file that doesn't exist");
#endif
            return null;
        }

        SettingsGeneralData data;
        using (var stream = new FileStream(path, FileMode.Open))
        {
            var formatter = new BinaryFormatter();

            data = formatter.Deserialize(stream) as SettingsGeneralData;
        }
#if UNITY_EDITOR
        Debug.Log("SettingsData has beem loaded successfully");
#endif
        CachedSave = data;
        return data;
    }

    public static bool Delete()
    {
        string path = Application.persistentDataPath + FileName;

        if (!File.Exists(path))
        {
#if UNITY_EDITOR
            Debug.LogError("Trying to access SettingsSave file that doesn't exist");
#endif
            return false;
        }

        File.Delete(path);

#if UNITY_EDITOR
        Debug.Log("SettingsData has beem deleted successfully");
#endif

        return true;
    }
}
