using UnityEngine;
using System.Collections;

public class AR : Weapon
{
    bool isAttackWindowOpen;
    public override void Awake()
    {
        weaponName = "AR";
        damage = 10;
        attackSpeed = 0.1f;
        attackCooldown = 0f;
        attackCollider = GetComponent<BoxCollider2D>();
        weaponVisual.enabled = false;
        weaponCollider.enabled = false;
        attackCooldown = 0f;
        isAttackWindowOpen = false;
    }

    public override void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown < 0f)
                attackCooldown = 0f;
            Debug.Log("Attack cooldown: " + attackCooldown);
        }
    }

    public override void Attack()
    {
        if (attackCooldown > 0f || isAttackWindowOpen)
            return;

        attackCooldown = attackSpeed;
        Debug.Log("Attack called");
        StartAttackWindow();
    }

    void StartAttackWindow()
    {
        if (attackRoutine != null)
            StopCoroutine(attackRoutine);

        attackRoutine = StartCoroutine(AttackWindow());
    }

    IEnumerator AttackWindow()
    {
        isAttackWindowOpen = true;
        weaponCollider.enabled = true;
        weaponVisual.enabled = true;

        yield return new WaitForSeconds(attackFlashDuration);

        weaponCollider.enabled = false;
        weaponVisual.enabled = false;
        isAttackWindowOpen = false;
        attackRoutine = null;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            Debug.Log("Hit enemy with " + weaponName);
        }
    }
}
