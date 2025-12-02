using UnityEngine;

/// Applies movement using the values read by PlayerInputRouter.
[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
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

    public void SetSpeedMultiplier(float multiplier)
    {
        moveSpeed = baseMoveSpeed * multiplier;
    }

    public float CurrentMoveSpeed => moveSpeed;
}
