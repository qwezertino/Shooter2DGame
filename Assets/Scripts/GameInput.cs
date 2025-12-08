using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSlotEventArgs : EventArgs
{
    public int SlotIndex { get; set; }
}

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions _playerInputActions;

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerStopAttack;
    public event EventHandler<WeaponSlotEventArgs> OnWeaponSlotChanged;

    [SerializeField] private Camera _camera;

    private void Awake()
    {
        // if (Instance != null)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        Instance = this;
        // DontDestroyOnLoad(gameObject);

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();
        // _playerInputActions.Combat.Attack.started += PlayerAttack_started;
        _playerInputActions.Combat.Attack.performed += PlayerAttack_started;
        _playerInputActions.Combat.Attack.canceled += PlayerAttack_canceled;

        _playerInputActions.Hotkeys.Keyboard.performed += Hotkeys_performed;
    }

    private void Hotkeys_performed(InputAction.CallbackContext obj)
    {
        float value = obj.ReadValue<float>();
        int slot = Mathf.RoundToInt(value);
        OnWeaponSlotChanged?.Invoke(this, new WeaponSlotEventArgs { SlotIndex = slot });
    }

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerAttack_canceled(InputAction.CallbackContext obj)
    {
        OnPlayerStopAttack?.Invoke(this, EventArgs.Empty);
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
        Vector3 mousePos = GetMousePosition();

        if (mousePos.x < 0 || mousePos.x > Screen.width ||
            mousePos.y < 0 || mousePos.y > Screen.height)
        {
            return _camera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        }

        Vector3 worldPos = _camera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        return worldPos;
    }
}
