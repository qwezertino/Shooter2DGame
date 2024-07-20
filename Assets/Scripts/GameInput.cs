using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions _playerInputActions;

    public event EventHandler OnPlayerAttack;

    [SerializeField] private Camera _camera;

    private void Awake()
    {
        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
    }

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMovementVector()
    {
        Vector2 movementVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return movementVector;
    }
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return mousePos;
    }
}
