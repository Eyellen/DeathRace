using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInput
{
    public static bool IsBlocked { get; set; }

    // Bools
    public static bool IsRightActionPressed { get => IsBlocked ? default(bool) : Input.GetMouseButton(1); }
    public static bool IsLeftActionPressed { get => IsBlocked ? default(bool) : Input.GetMouseButton(0); }

    // Floats
    public static float HorizontalAxis { get => IsBlocked ? default(float) : Input.GetAxisRaw("Horizontal"); }
    public static float VerticalAxis { get => IsBlocked ? default(float) : Input.GetAxisRaw("Vertical"); }
    public static float Brake { get => IsBlocked ? default(float) : Input.GetAxisRaw("Brake"); }
}
