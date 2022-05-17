using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Bool
    public bool IsForwardPressed { get; private set; }
    public bool IsBackwardPressed { get; private set; }
    public bool IsRightPressed { get; private set; }
    public bool IsLeftPressed { get; private set; }
    public bool IsSpacePressed { get; private set; }

    // Float
    public float HorizontalAxis { get; private set; }
    public float VerticalAxis { get; private set; }
    public float Brake { get; private set; }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Zeroing values
        IsForwardPressed = false;
        IsBackwardPressed = false;
        IsRightPressed = false;
        IsLeftPressed = false;
        IsSpacePressed = false;

        // Input
        if (Input.GetKey(KeyCode.W))
        {
            IsForwardPressed = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            IsBackwardPressed = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            IsRightPressed = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            IsLeftPressed = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            IsSpacePressed = true;
        }
        HorizontalAxis = Input.GetAxisRaw("Horizontal");
        VerticalAxis = Input.GetAxisRaw("Vertical");
        Brake = Input.GetAxisRaw("Brake");
    }
}
