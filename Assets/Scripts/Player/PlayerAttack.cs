using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerInputRouter inputRouter;
    [SerializeField] float attackRange = 1f;
    [SerializeField] LayerMask enemyLayer;

    BoxCollider2D attackCollider;

    void Reset()
    {
        inputRouter = GetComponent<PlayerInputRouter>();
        attackCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (inputRouter == null)
            return;

        if (inputRouter.AttackPressed)
            Attack();

        DrawAimDebugLine();
    }

    void DrawAimDebugLine()
    {
        if (Camera.main == null || Mouse.current == null)
            return;

        Vector3 mouseScreen = new Vector3(Mouse.current.position.ReadValue().x,
                                          Mouse.current.position.ReadValue().y,
                                          Mathf.Abs(Camera.main.transform.position.z - transform.position.z));
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        Debug.DrawLine(transform.position, mouseWorld, Color.green);
    }

    void Attack()
    {
        Debug.Log("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (inputRouter == null || Camera.main == null)
            return;

        Vector3 mouseScreen = new Vector3(inputRouter.MouseValue.x,
                                          inputRouter.MouseValue.y,
                                          Mathf.Abs(Camera.main.transform.position.z - transform.position.z));
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, mouseWorld);
    }
}
