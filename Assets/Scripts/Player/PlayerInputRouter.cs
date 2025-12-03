using UnityEngine;
using UnityEngine.InputSystem;

/// Keeps all player-input handling in one place.
public class PlayerInputRouter : MonoBehaviour
{
    public Vector2 MoveValue { get; private set; }
    public Vector2 MouseValue { get; private set; }
    public bool AttackPressed { get; private set; }
    public bool UseItemPressed { get; private set; }
    public bool OpenInventoryPressed { get; private set; }
    public bool DropItemPressed { get; private set; }

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

        actions.Player.Look.performed += OnLook;
        actions.Player.Look.canceled += OnLook;

        actions.Player.Attack.performed += OnAttack;
        actions.Player.Attack.canceled += OnAttack;

        actions.Player.UseItem.performed += OnUseItem;
        actions.Player.OpenInventory.performed += OnOpenInventory;
        actions.Player.DropItem.performed += OnDropItem;
    }

    void OnDisable()
    {
        actions.Player.Move.performed -= OnMove;
        actions.Player.Move.canceled -= OnMove;

        actions.Player.Look.performed -= OnLook;
        actions.Player.Look.canceled -= OnLook;

        actions.Player.Attack.performed -= OnAttack;
        actions.Player.Attack.canceled -= OnAttack;

        actions.Player.UseItem.performed -= OnUseItem;
        actions.Player.OpenInventory.performed -= OnOpenInventory;
        actions.Player.DropItem.performed -= OnDropItem;

        actions.Player.Disable();
    }

    // Add a LateUpdate to reset the press flags each frame
    void LateUpdate()
    {
        UseItemPressed = false;
        OpenInventoryPressed = false;
        DropItemPressed = false;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        MoveValue = ctx.ReadValue<Vector2>();
    }

    void OnLook(InputAction.CallbackContext ctx)
    {
        MouseValue = ctx.ReadValue<Vector2>();
    }

    void OnAttack(InputAction.CallbackContext ctx)
    {
        AttackPressed = ctx.ReadValueAsButton();
    }

    void OnUseItem(InputAction.CallbackContext ctx)
    {
        UseItemPressed = true;
    }

    void OnOpenInventory(InputAction.CallbackContext ctx)
    {
        OpenInventoryPressed = true;
    }

    void OnDropItem(InputAction.CallbackContext ctx)
    {
        DropItemPressed = true;
    }
}
