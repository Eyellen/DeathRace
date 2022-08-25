using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInput
{
    // Blocks all inputs
    public static bool IsBlocked { get; set; }

    // Blocks only buttons (doesn't block mouse movement)
    public static bool IsButtonsBlocked { get; set; }

    // Blocks only mouse axis
    public static bool IsMouseAxisBlocked { get; set; }

    // Bools
    public static bool IsRightActionPressed { get => CheckIfButtonsBlocked(Input.GetMouseButton(1)); }
    public static bool IsLeftActionPressed { get => CheckIfButtonsBlocked(Input.GetMouseButton(0)); }

    public static bool IsLightsPressed { get => CheckIfButtonsBlocked(Input.GetKeyDown(KeyCode.H)); }
    public static bool IsBackViewHolding { get => CheckIfButtonsBlocked(Input.GetKey(KeyCode.C)); }
    public static bool IsReleasedBackView { get => CheckIfButtonsBlocked(Input.GetKeyUp(KeyCode.C)); }

    public static bool IsSmokePressed { get => CheckIfButtonsBlocked(Input.GetKeyDown(KeyCode.X)); }
    public static bool IsDropBackPlatePressed { get => CheckIfButtonsBlocked(Input.GetKeyDown(KeyCode.Z)); }

    public static bool IsChatPressed { get => CheckIfButtonsBlocked(Input.GetKeyDown(KeyCode.T)); }

    public static bool IsRespawnPressed { get => CheckIfButtonsBlocked(Input.GetKeyDown(KeyCode.R)); }
    public static bool IsStartGamePressed { get => CheckIfButtonsBlocked(Input.GetKeyDown(KeyCode.P)); }
    public static bool IsEscapePressed { get => CheckIfButtonsBlocked(Input.GetKeyDown(KeyCode.Escape)); }

    // Floats
    public static float MouseHorizontalAxis { get => CheckIfNouseAxisBlocked(Input.GetAxisRaw("Mouse X")); }
    public static float MouseVerticalAxis { get => CheckIfNouseAxisBlocked(Input.GetAxisRaw("Mouse Y")); }
    public static float HorizontalAxis { get => CheckIfButtonsBlocked(Input.GetAxisRaw("Horizontal")); }
    public static float VerticalAxis { get => CheckIfButtonsBlocked(Input.GetAxisRaw("Vertical")); }
    public static float UpDownAxis { get => CheckIfButtonsBlocked(Input.GetAxisRaw("UpDown")); }
    public static float Brake { get => CheckIfButtonsBlocked(Input.GetAxisRaw("Brake")); }

    private static T CheckIfButtonsBlocked<T>(T value)
    {
        return (IsBlocked || IsButtonsBlocked) ? default(T) : value;
    }

    private static T CheckIfNouseAxisBlocked<T>(T value)
    {
        return (IsBlocked || IsMouseAxisBlocked) ? default(T) : value;
    }
}
