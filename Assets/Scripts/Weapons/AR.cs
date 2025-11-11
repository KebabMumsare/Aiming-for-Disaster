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

        float timeRemaining = attackFlashDuration;
        while (timeRemaining > 0f)
        {
            timeRemaining -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        weaponCollider.enabled = false;
        weaponVisual.enabled = false;
        isAttackWindowOpen = false;
        attackRoutine = null;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D: " + other.gameObject.name);
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit enemy " + other.gameObject.name + " with " + damage + " damage");
            other.gameObject.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
