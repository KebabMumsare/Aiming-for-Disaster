using UnityEngine;
using System.Collections;
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public string weaponName;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected Collider2D attackCollider;
    [SerializeField] protected LayerMask enemyLayer;

    [SerializeField] SpriteRenderer weaponVisual;
    [SerializeField] Collider2D weaponCollider;
    [SerializeField] float attackFlashDuration = 0.1f;
    Coroutine attackRoutine;

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

    public abstract void Attack();
    public abstract void Awake();
    public abstract void OnTriggerEnter2D(Collider2D other);
}