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

    private static void UpdateCachedSave()
    {
        CachedSave = new SettingsGeneralData();
    }
    #endregion

    public static void Save()
    {
        string path = Application.persistentDataPath + FileName;

        var stream = new FileStream(path, FileMode.Create);

        var formatter = new BinaryFormatter();

        var data = new SettingsGeneralData();

        formatter.Serialize(stream, data);

        stream.Close();

#if UNITY_EDITOR
        Debug.Log("SettingsData has beem saved successfully");
#endif
        UpdateCachedSave();
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

        var stream = new FileStream(path, FileMode.Open);

        var formatter = new BinaryFormatter();

        SettingsGeneralData data = formatter.Deserialize(stream) as SettingsGeneralData;

        stream.Close();

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
