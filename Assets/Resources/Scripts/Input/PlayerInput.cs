using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set; }

    // Bool
    public bool IsForwardPressed { get; private set; }
    public bool IsBackwardPressed { get; private set; }
    public bool IsRightPressed { get; private set; }
    public bool IsLeftPressed { get; private set; }
    public bool IsSpacePressed { get; private set; }
    public bool IsRightActionPressed { get; private set; }
    public bool IsLeftActionPressed { get; private set; }

    // Float
    public float HorizontalAxis { get; private set; }
    public float VerticalAxis { get; private set; }
    public float Brake { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogError($"Trying to create another one {nameof(PlayerInput)} on {transform.name} when it is a Singleton. " +
                $"Destroyed {transform.name} to prevent this.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Input
        IsForwardPressed = Input.GetKey(KeyCode.W);
        IsBackwardPressed = Input.GetKey(KeyCode.S);
        IsRightPressed = Input.GetKey(KeyCode.D);
        IsLeftPressed = Input.GetKey(KeyCode.A);
        IsSpacePressed = Input.GetKey(KeyCode.Space);

        IsRightActionPressed = Input.GetMouseButton(1);
        IsLeftActionPressed = Input.GetMouseButton(0);

        HorizontalAxis = Input.GetAxisRaw("Horizontal");
        VerticalAxis = Input.GetAxisRaw("Vertical");
        Brake = Input.GetAxisRaw("Brake");
    }
}
