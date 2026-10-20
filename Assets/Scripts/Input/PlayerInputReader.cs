using UnityEngine;
using System;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputReader", menuName = "ModularAssets/PlayerInputReader")]
public class PlayerInputReader : ScriptableObject
{
    public event Action InteractEvent;
    public event Action FireEvent;
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput => _controls.Player.Look.ReadValue<Vector2>();

    private InputSystem_Actions _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new InputSystem_Actions();
        }
        _controls.Player.Move.performed += HandleMove;
        _controls.Player.Move.canceled += HandleMove;
        _controls.Player.Interact.performed += HandleInteract;
        _controls.Player.Attack.performed += HandleFire;
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Move.performed -= HandleMove;
        _controls.Player.Move.canceled -= HandleMove;
        _controls.Player.Interact.performed -= HandleInteract;
        _controls.Player.Attack.performed -= HandleFire;
        _controls.Player.Disable();
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }

    private void HandleFire(InputAction.CallbackContext context)
    {
        FireEvent?.Invoke();
    }
}