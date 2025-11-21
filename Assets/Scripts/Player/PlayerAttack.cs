using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerInputRouter inputRouter;
    [SerializeField] Weapon weapon;
    [SerializeField] Transform weaponHoldingPoint;

    [SerializeField] Transform weaponPivot;

    bool previousAttackHeld;

    void Reset()
    {
        inputRouter = GetComponent<PlayerInputRouter>();
        //attackCollider = GetComponent<BoxCollider2D>();
    }
    

    void Update()
    {
        if (weapon == null)
        {
            weapon = weaponPivot.GetComponentInChildren<Weapon>();
            if (weapon == null)
            {
                return;
            }
            else 
            {
                Debug.Log("Weapon found in weaponPivot");
                weaponHoldingPoint = weapon.transform;
            }
        }
        else if (!weapon.IsEquipped)
        {
            weapon = null;
            weaponHoldingPoint = null;
            return;
        }


        if (inputRouter == null)
            return;

        bool attackHeld = inputRouter.AttackPressed;

        if (weapon != null && attackHeld && !previousAttackHeld)
            weapon.Attack();     // call once when the button is first pressed

        previousAttackHeld = attackHeld;

        DrawAimDebugLine();
        UpdateWeaponAim();
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
        //Debug.Log("Attack");
        if (weapon != null)
        {
            weapon.Attack();
        }
    }

    void UpdateWeaponAim()
    {
        if (Camera.main == null || Mouse.current == null || weaponPivot == null)
            return;

        Vector3 mouseScreen = new Vector3(Mouse.current.position.ReadValue().x,
                                          Mouse.current.position.ReadValue().y,
                                          Mathf.Abs(Camera.main.transform.position.z - weaponPivot.position.z));
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);

        Vector2 aimDirection = (mouseWorld - weaponPivot.position).normalized;
        weaponPivot.up = aimDirection;          // or .up if your art points up
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
