using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "ModularAssets/HoverInputReader")]
public class HoverInputReader : ScriptableObject
{
    public event UnityAction<Vector2> MoveEvent = delegate { };

    public Vector2 MoveInput { get; private set; }

    private InputSystem_Actions _actions;

    private void OnEnable()
    {
        if (_actions == null) _actions = new InputSystem_Actions();

        _actions.Player.Move.performed += ctx =>
        {
            MoveInput = ctx.ReadValue<Vector2>();
            MoveEvent.Invoke(MoveInput);
        };
        _actions.Player.Move.canceled += ctx =>
        {
            MoveInput = Vector2.zero;
            MoveEvent.Invoke(MoveInput);
        };

        _actions.Player.Enable();
    }

    private void OnDisable()
    {
        if (_actions != null) _actions.Player.Disable();
    }
}
