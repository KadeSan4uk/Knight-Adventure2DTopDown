using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        TakePlayerMovementControl();
    }

    private void TakePlayerMovementControl()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public Vector3 GetMousPosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
}
