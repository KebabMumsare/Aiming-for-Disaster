using UnityEngine;
using System.Collections;

public class BossBehaviourController : EnemyBehaviorController
{
    public Health health;

    [Header("Ranged Settings")]
    public GameObject projectilePrefab;
    public float attackRange = 8f;
    public float keepDistance = 6f;
    public float fireRate = 2f;
    public float projectileSpeed = 10f;
    public Transform firePoint;
    
    [Header("Stage 2 - Melee Settings")]
    public float meleeRange = 3f;
    public float meleeAttackCooldown = 1.5f;
    public float chargeSpeed = 8f;
    public float chargeDistance = 5f;
    public Collider2D meleeAttackCollider;
    
    [Header("Stage 3 - Frenzy Settings")]
    public float frenzyFireRate = 0.5f;
    public int projectilesPerBurst = 3;
    public float burstSpreadAngle = 30f;
    public float frenzyMovementSpeed = 6f;
    
    public enum BossStage
    {
        Stage1,
        Stage2,
        Stage3
    }

    [Header("Stage Settings")]
    public BossStage currentStage = BossStage.Stage1;

    private float nextFireTime;
    private float nextMeleeAttackTime;
    private bool isCharging = false;
    private Vector3 chargeTarget;

    protected override void Update()
    {
        base.Update();

        if (playerTransform == null) return;

        switch (currentStage)
        {
            case BossStage.Stage1:
                StageOneAttack();
                break;
            case BossStage.Stage2:
                StageTwoAttack();
                break;
            case BossStage.Stage3:
                StageThreeAttack();
                break;
        }
    }

    private void StageOneAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Keep distance from player while attacking
        if (distanceToPlayer < keepDistance)
        {
            Vector3 directionAway = (transform.position - playerTransform.position).normalized;
            if (ai != null)
            {
                ai.destination = transform.position + directionAway * 2f;
            }
        }
        else if (distanceToPlayer > attackRange)
        {
            // Move closer if too far
            if (ai != null)
            {
                ai.destination = playerTransform.position;
            }
        }

        // Attack with single projectile
        if (distanceToPlayer <= attackRange && Time.time >= nextFireTime)
        {
            if (projectilePrefab != null)
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                Shoot(direction);
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    private void StageTwoAttack()
    {
        // Implement Stage 2 logic here
        // Example: Melee attack or different pattern
    }

    private void StageThreeAttack()
    {
        // Implement Stage 3 logic here
        // Example: Frenzy mode or special ability
    }

    void Shoot(Vector3 direction)
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        spawnPos.z = -7; // Ensure z is set correctly for 2D
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        
        // Rotate projectile to face direction
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90); // Assuming projectile sprite points UP

        EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
        if (ep != null)
        {
            ep.speed = projectileSpeed;
        }
    }
}
