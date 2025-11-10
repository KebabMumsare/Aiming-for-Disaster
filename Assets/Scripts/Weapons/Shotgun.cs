using UnityEngine;
using System.Collections;

public class Shotgun : Weapon
{
    bool isAttackWindowOpen;
    [SerializeField] MeshRenderer spreadVisual; 
    public override void Awake()
    {
        weaponName = "Shotgun";
        damage = 10;
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
        //weaponVisual.enabled = true;
        spreadVisual.enabled = true;

        yield return new WaitForSeconds(attackFlashDuration);

        weaponCollider.enabled = false;
        //weaponVisual.enabled = false;
        spreadVisual.enabled = false;
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
