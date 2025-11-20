using UnityEngine;
using System.Collections;
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public string weaponName;
    [SerializeField] protected Damage damage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected Collider2D attackCollider;
    [SerializeField] protected LayerMask enemyLayer;

    [SerializeField] protected SpriteRenderer weaponVisual;
    [SerializeField] protected Collider2D weaponCollider;
    [SerializeField] protected float attackFlashDuration = 0.1f;
    protected Coroutine attackRoutine;
    protected Transform equippedPivot;
    protected bool isEquipped;

    public void TriggerAttackFlash()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(AttackWindow());
    }

    IEnumerator AttackWindow()
    {
        weaponVisual.enabled = true;
        weaponCollider.enabled = true;

        yield return new WaitForSeconds(attackFlashDuration);

        weaponCollider.enabled = false;
        weaponVisual.enabled = false;
        attackRoutine = null;
    }

    public virtual void OnEquipped(Transform pivot)
    {
        equippedPivot = pivot;
        isEquipped = true;
    }

    public virtual void OnUnequipped()
    {
        isEquipped = false;
        equippedPivot = null;
    }

    public abstract void Awake();
    public abstract void Update();
    public abstract void Attack();
    public abstract void OnTriggerEnter2D(Collider2D other);
}