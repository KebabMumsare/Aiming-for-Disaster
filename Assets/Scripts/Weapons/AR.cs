using UnityEngine;

public class AR : Weapon
{
    public override void Awake()
    {
        //base.Awake();
        weaponName = "AR";
        damage = 10;
        attackSpeed = 0.1f;
        attackCooldown = 0f;
        attackCollider = GetComponent<BoxCollider2D>();
    }

    public override void Attack()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            Debug.Log("Attack cooldown: " + attackCooldown);
            return;
        }
        TriggerAttackFlash();
        Debug.Log("Attacking with " + weaponName);
        attackCooldown = attackSpeed;
        
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            Debug.Log("Hit enemy with " + weaponName);
        }
    }
}
