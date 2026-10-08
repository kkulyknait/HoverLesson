using UnityEngine;
using System;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputReader", menuName = "ModularAssets/PlayerInputReader")]
public class PlayerInputReader : ScriptableObject
{
    public event Action InteractEvent;
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    private InputSystem_Actions _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new InputSystem_Actions(); 
        }
        _controls.Player.Move.performed += HandleMove;
        _controls.Player.Move.canceled += HandleMove;
        _controls.Player.Look.performed += HandleLook;
        _controls.Player.Look.canceled += HandleLook;
        _controls.Player.Interact.performed += HandleInteract;
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Move.performed -= HandleMove;
        _controls.Player.Move.canceled -= HandleMove;
        _controls.Player.Look.performed -= HandleLook;
        _controls.Player.Look.canceled -= HandleLook;
        _controls.Player.Interact.performed -= HandleInteract;
        _controls.Player.Disable();
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
    private void HandleLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }
    private void HandleInteract(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }

}
