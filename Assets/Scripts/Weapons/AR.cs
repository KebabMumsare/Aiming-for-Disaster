using UnityEngine;
using System.Collections;

public class AR : Weapon
{
    bool isAttackWindowOpen;
    [SerializeField] Item item;
    public override void Awake()
    {
        weaponName = "AR";
        damage = GetComponent<Damage>();
        attackSpeed = attackSpeed; // fetches the attack speed from the Weapon.cs
        attackCooldown = 0f;
        attackCollider = GetComponent<BoxCollider2D>();
        weaponVisual.enabled = false;
        weaponCollider.enabled = false;
        isAttackWindowOpen = false;
    }

    public override void Update()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown < 0f)
                attackCooldown = 0f;
        }
    }

    public override void Attack()
    {
        if (attackCooldown > 0f || isAttackWindowOpen)
            return;

        attackCooldown = attackSpeed;
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
        Debug.Log("OnTriggerEnter2D: " + other.gameObject.name); // Unnecessary
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit enemy " + other.gameObject.name + " with " + damage.GetDamage() + " damage"); // Unnecessary
            other.gameObject.GetComponent<Health>().TakeDamage(damage.GetDamage());
        }
        if (other.gameObject.tag == "LootBox")
        {
            Debug.Log("Hit enemy " + other.gameObject.name + " with " + damage.GetDamage() + " damage"); // Unnecessary
            other.gameObject.GetComponent<Health>().TakeDamage(damage.GetDamage());
        }
    }
}
