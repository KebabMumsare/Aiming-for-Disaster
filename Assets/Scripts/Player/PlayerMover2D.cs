using UnityEngine;

/// Applies movement using the values read by PlayerInputRouter.
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMover2D : MonoBehaviour
{
    [SerializeField] PlayerInputRouter inputRouter;
    [SerializeField] float moveSpeed = 5f;

    void Reset()
    {
        inputRouter = GetComponent<PlayerInputRouter>();
    }

    void Update()
    {
        if (inputRouter == null)
            return;

        Vector2 moveInput = inputRouter.MoveValue;

        if (moveInput.sqrMagnitude > 1f)
            moveInput.Normalize();

        Vector3 displacement = (Vector3)(moveInput * moveSpeed * Time.deltaTime);
        transform.position += displacement;
    }
}
