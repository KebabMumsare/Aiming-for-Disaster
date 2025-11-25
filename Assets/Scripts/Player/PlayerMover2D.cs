using UnityEngine;

/// Applies movement using the values read by PlayerInputRouter.
[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class PlayerMover2D : MonoBehaviour
{
    [SerializeField] PlayerInputRouter inputRouter;
    [SerializeField] float moveSpeed = 5f;

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
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        moveSpeed = baseMoveSpeed * multiplier;
    }
}
