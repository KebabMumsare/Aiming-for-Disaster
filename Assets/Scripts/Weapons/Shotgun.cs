using UnityEngine;
using System.Collections;

public class Shotgun : Weapon
{
    bool isAttackWindowOpen;
    [SerializeField] MeshRenderer spreadVisual; 
    public override void Awake()
    {
        weaponName = "Shotgun";
        damage = GetComponent<Damage>();
        attackSpeed = 0.5f;
        attackCooldown = 1f;
        attackCollider = GetComponent<BoxCollider2D>();
        //weaponVisual.enabled = false;
        weaponCollider.enabled = false;
        attackCooldown = 0f;
        isAttackWindowOpen = false;
        spreadVisual.enabled = false;
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
        //weaponVisual.enabled = true;
        spreadVisual.enabled = true;

        float timeRemaining = attackFlashDuration;
        while (timeRemaining > 0f)
        {
            timeRemaining -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        weaponCollider.enabled = false;
        //weaponVisual.enabled = false;
        spreadVisual.enabled = false;
        isAttackWindowOpen = false;
        attackRoutine = null;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D: " + other.gameObject.name);
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit enemy " + other.gameObject.name + " with " + damage.GetDamage() + " damage");
            other.gameObject.GetComponent<Health>().TakeDamage(damage.GetDamage());
        }
    }
}
