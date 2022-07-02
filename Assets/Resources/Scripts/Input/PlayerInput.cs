using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInput
{
    public static bool IsBlocked { get; set; }

    // Bools
    public static bool IsRightActionPressed { get => Condition(Input.GetMouseButton(1)); }
    public static bool IsLeftActionPressed { get => Condition(Input.GetMouseButton(0)); }

    // Floats
    public static float MouseHorizontalAxis { get => Condition(Input.GetAxis("Mouse X")); }
    public static float MouseVerticalAxis { get => Condition(Input.GetAxis("Mouse Y")); }
    public static float HorizontalAxis { get => Condition(Input.GetAxisRaw("Horizontal")); }
    public static float VerticalAxis { get => Condition(Input.GetAxisRaw("Vertical")); }
    public static float Brake { get => Condition(Input.GetAxisRaw("Brake")); }

    private static T Condition<T>(T value)
    {
        return IsBlocked ? default(T) : value;
    }
}
