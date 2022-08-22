using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SettingsUser
{
    private static string s_username = "Driver";

    private static bool s_isSensitivitySplit;
    private static Vector2 s_sensitivity = new Vector2(0.2f, 0.2f);

    public static string Username { get => s_username; }
    public static bool IsSensitivitySplit { get => s_isSensitivitySplit; }
    public static Vector2 Sensitivity { get => s_sensitivity; }

    public delegate void SensitivityChangedEvent();
    public static event SensitivityChangedEvent OnSensitivityChanged;

    public static void SetUsername(string username)
    {
        s_username = username;
    }

    public static void SetSensitivitySplit(bool isSplitted)
    {
        s_isSensitivitySplit = isSplitted;

        if (!isSplitted)
        {
            s_sensitivity = new Vector2(s_sensitivity.x, s_sensitivity.x);
        }
    }

    public static void SetSensitivity(float value)
    {
        s_sensitivity.x = s_sensitivity.y = value;
        OnSensitivityChanged?.Invoke();
    }

    public static void SetSensitivity(Vector2 sensitivity)
    {
        s_sensitivity = sensitivity;
    }

    public static void SetXSensitivity(float value)
    {
        s_sensitivity.x = value;
        OnSensitivityChanged?.Invoke();
    }

    public static void SetYSensitivity(float value)
    {
        s_sensitivity.y = value;
        OnSensitivityChanged?.Invoke();
    }
}
