using UnityEngine;

/// Applies movement using the values read by PlayerInputRouter.
[RequireComponent(typeof(CapsuleCollider2D), typeof(Rigidbody2D))]
public class PlayerMover2D : MonoBehaviour
{
    [SerializeField] PlayerInputRouter inputRouter;
    [SerializeField] float moveSpeed = 5f;

    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";

    private Animator _animator;

    Rigidbody2D rb;

    void Reset()
    {
        inputRouter = GetComponent<PlayerInputRouter>();
    }

    private float baseMoveSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseMoveSpeed = moveSpeed;
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (inputRouter == null)
            return;

        Vector2 moveInput = inputRouter.MoveValue;

        if (moveInput.sqrMagnitude > 1f)
            moveInput.Normalize();

        // Set velocity directly - will come to full stop when moveInput is zero
        rb.linearVelocity = moveInput * moveSpeed;

        _animator.SetFloat(_horizontal, moveInput.x);
        _animator.SetFloat(_vertical, moveInput.y);
    }

    private float skillMultiplier = 1f;
    private float itemMultiplier = 1f;

    public void SetItemSpeedMultiplier(float multiplier)
    {
        itemMultiplier = multiplier;
        UpdateMoveSpeed();
    }

    public void SetSkillSpeedMultiplier(float multiplier)
    {
        skillMultiplier = multiplier;
        UpdateMoveSpeed();
    }

    private void UpdateMoveSpeed()
    {
        // Additive scaling: Base + (Base * (Skill - 1)) + (Base * (Item - 1))
        // Simplified: Base * (Skill + Item - 1)
        moveSpeed = baseMoveSpeed * (skillMultiplier + itemMultiplier - 1f);
    }

    public float CurrentMoveSpeed => moveSpeed;
}
