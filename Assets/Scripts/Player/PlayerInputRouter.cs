using UnityEngine;
using UnityEngine.InputSystem;

/// Keeps all player-input handling in one place.
public class PlayerInputRouter : MonoBehaviour
{
    public Vector2 MoveValue { get; private set; }

    PlayerInputActions actions;

    void Awake()
    {
        actions = new PlayerInputActions();
    }

    void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMove;
        actions.Player.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        actions.Player.Move.performed -= OnMove;
        actions.Player.Move.canceled -= OnMove;
        actions.Player.Disable();
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        MoveValue = ctx.ReadValue<Vector2>();
    }
}
